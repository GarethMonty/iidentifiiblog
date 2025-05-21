namespace IIdentifii.Blog.BusinessLogic
{
    public class RequestContextService : IRequestContextService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Properties

        public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;

        public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value;

        #endregion

        #region Constructor Methods

        public RequestContextService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        public bool TryGetUserId(
            out Guid userId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                userId = Guid.Empty;
                return false;
            }

            return Guid.TryParse(UserId, out userId);
        }

        #endregion
    }
}
