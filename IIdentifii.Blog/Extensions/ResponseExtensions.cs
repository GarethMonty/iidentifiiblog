namespace IIdentifii.Blog.Shared
{
    public static class ResponseExtensions
    {
        public static IActionResult ToResult<T>(this ApiResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.Code
            };
        }
    }
}
