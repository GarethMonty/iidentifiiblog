namespace IIdentifii.Blog.Shared
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool IsSuccess {  get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("error")]
        public string? ErrorMessage { get; set; }

        [JsonIgnore]
        public bool HasData => Data != null;

        [JsonIgnore]
        public Exception? Exception { get; set; }

        #region Static Methods

        public static ApiResponse<T> Success(
            T data, 
            int code = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Code = code,
                Data = data
            };
        }

        public static ApiResponse<T> Failure(
            string errorMessage, 
            int code = 400, 
            Exception? exception = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Code = code,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        public static ApiResponse<T> NotFound(
            string errorMessage, 
            int code = 404, 
            Exception? exception = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Code = code,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        public static ApiResponse<T> Unauthorized(
            string errorMessage, 
            int code = 401, 
            Exception? exception = null)
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
