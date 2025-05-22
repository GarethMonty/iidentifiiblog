namespace IIdentifii.Blog
{
    public class GlobalExceptionMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor Methods

        public GlobalExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Methods

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (IIdentifiiException iiEx)
            {
                await CreateErrorResponse(context, iiEx.Message, iiEx.Code, iiEx);
            }
            catch (Exception ex)
            {
                await CreateErrorResponse(context, ex.Message, StatusCodes.Status500InternalServerError, ex);
            }
        }

        private static async Task CreateErrorResponse(
            HttpContext context,
            string message,
            int statusCode,
            Exception exception)
        {
            ApiResponse<object> errorResponse = ApiResponse<object>.Failure(
                message,
                statusCode,
                exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        #endregion
    }
}
