namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// A standard API response wrapper used for all endpoints.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the API request was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The HTTP status code associated with the response (e.g. 200, 400, 404).
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// The response payload data (e.g. an object, list, or result value).
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        /// <summary>
        /// The error message returned when the request fails.
        /// </summary>
        [JsonPropertyName("error")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// The list of errors returned when the request fails.
        /// </summary>
        [JsonPropertyName("errors")]
        public List<string>? Errors { get; set; } = null;

        /// <summary>
        /// Indicates whether the response contains data.
        /// </summary>
        [JsonIgnore]
        public bool HasData => Data != null;

        /// <summary>
        /// The exception instance (if any), only used internally for debugging/logging.
        /// Not included in the serialized response.
        /// </summary>
        [JsonIgnore]
        public Exception? Exception { get; set; }

        #region Static Methods

        /// <summary>
        /// Returns a successful response with the provided data.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="code">Optional status code (defaults to 200).</param>
        public static ApiResponse<T> Success(T data, int code = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Code = code,
                Data = data
            };
        }

        /// <summary>
        /// Returns a failure response with an error message and optional exception.
        /// </summary>
        /// <param name="errorMessage">The error message to include.</param>
        /// <param name="errors">The errors to include</param>
        /// <param name="code">The HTTP status code (default is 400).</param>
        /// <param name="exception">Optional exception to log or debug with.</param>
        public static ApiResponse<T> Failure(string errorMessage, List<string>? errors = null, int code = 400, Exception? exception = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Code = code,
                ErrorMessage = errorMessage,
                Errors = errors,
                Exception = exception
            };
        }

        /// <summary>
        /// Returns a 404 Not Found failure response.
        /// </summary>
        public static ApiResponse<T> NotFound(string errorMessage, int code = 404, Exception? exception = null)
        {
            return new ApiResponse<T>
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
        public static ApiResponse<T> Unauthorized(string errorMessage, int code = 401, Exception? exception = null)
        {
            return new ApiResponse<T>
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
