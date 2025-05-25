namespace IIdentifii.Blog.BusinessLogic
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse?>> GenerateJwtToken(
            LoginRequest request);
    }
}