using static System.Net.WebRequestMethods;

namespace IIdentifii.Blog.BusinessLogic
{
    public class RequestContextService : IRequestContextService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Properties

        public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value ?? _httpContextAccessor.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value ?? _httpContextAccessor.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

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
