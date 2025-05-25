namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// A standard API response wrapper used for all endpoints.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned.</typeparam>
    public class PagedApiResponse<T> : ApiResponse<List<T>>
    {
        /// <summary>
        /// The response payload data list
        /// </summary>
        [JsonPropertyName("data")]
        public new List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// The page number (starting from 1).
        /// </summary>
        [Range(0, int.MaxValue, MinimumIsExclusive = true, MaximumIsExclusive = false)]
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of items to return per page (1–100).
        /// </summary>
        [Range(0, 100, MinimumIsExclusive = true, MaximumIsExclusive = true)]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// The minimum number of items to return per page
        /// </summary>
        [MinLength(0)]
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        #region Static Methods

        /// <summary>
        /// Returns a successful response with the provided data.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="totalCount">The total number of items available.</param>
        /// <param name="code">Optional status code (defaults to 200).</param>
        public static PagedApiResponse<T> Success(
            List<T> data, 
            int page, 
            int size, 
            int totalCount, 
            int code = 200)
        {
            return new PagedApiResponse<T>
            {
                IsSuccess = true,
                Code = code,
                Data = data,
                Page = page,
                PageSize = size,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Returns a failure response with an error message and optional exception.
        /// </summary>
        /// <param name="errorMessage">The error message to include.</param>
        /// <param name="code">The HTTP status code (default is 400).</param>
        /// <param name="exception">Optional exception to log or debug with.</param>
        public static PagedApiResponse<T> Failure(
            string errorMessage, 
            int code = 400, 
            Exception? exception = null)
        {
            return new PagedApiResponse<T>
            {
                IsSuccess = false,
                Code = code,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        /// <summary>
        /// Returns a 404 Not Found failure response.
        /// </summary>
        public static PagedApiResponse<T> NotFound(string errorMessage, int code = 404, Exception? exception = null)
        {
            return new PagedApiResponse<T>
            {
                IsSuccess = false,
                Code = code,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        /// <summary>
        /// Returns a 401 Unauthorized failure response.
        /// </summary>
        public static PagedApiResponse<T> Unauthorized(
            string errorMessage, 
            int code = 401, 
            Exception? exception = null)
        {
            return new PagedApiResponse<T>
            {
                IsSuccess = false,
                Code = code,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        #endregion
    }
}
