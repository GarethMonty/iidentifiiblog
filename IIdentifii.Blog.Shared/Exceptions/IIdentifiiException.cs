namespace IIdentifii.Blog.Shared
{
    public class IIdentifiiException : Exception
    {
        #region Properties

        public int Code { get; set; } = 500;

        public List<string>? Errors { get; set; } = null;

        public bool IsRetryable { get; set; } = false;

        #endregion

        #region Constructor Methods

        public IIdentifiiException(
            string message)
        : base(message)
        {
        }

        public IIdentifiiException(
            string message,
            Exception innerException)
        : base(message, innerException)
        {
        }

        #endregion

        #region Static Methods

        public static IIdentifiiException Create(
            string message,
            Exception innerException,
            int code = 500,
            List<string>? errors = null,
            bool isRetryable = false)
        {
            return new IIdentifiiException(message, innerException)
            {
                Code = code,
                Errors = errors,
                IsRetryable = isRetryable
            };
        }


        public static IIdentifiiException Create(
            string message,
            int code = 500,
            List<string>? errors = null,
            bool isRetryable = false)
        {
            return new IIdentifiiException(message)
            {
                Code = code,
                Errors = errors,
                IsRetryable = isRetryable
            };
        }
        #endregion
    }
}
