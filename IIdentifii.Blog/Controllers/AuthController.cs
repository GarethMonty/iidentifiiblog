using IIdentifii.Blog.BusinessLogic;
using IIdentifii.Blog.Extensions;
using IIdentifii.Blog.Shared;
using Microsoft.AspNetCore.Mvc;

namespace IIdentifii.Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Fields

        private readonly IAuthService _authService;

        #endregion

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest login)
        {
            ArgumentNullException.ThrowIfNull(login, nameof(login));

            ApiResponse<LoginResponse?> response = await _authService.GenerateJwtToken(login);

            return response.ToResult();
        }

    }
}
