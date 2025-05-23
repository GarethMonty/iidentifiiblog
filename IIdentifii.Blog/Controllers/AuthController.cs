namespace IIdentifii.Blog.Controllers
{
    /// <summary>
    /// Controller for handling API autentication requests.
    /// </summary>    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Fields

        private readonly IAuthService _authService;

        #endregion

        #region Constructor Methods

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        #endregion

        /// <summary>
        /// Authenticates a user and returns a JWT token and refresh token if successful.
        /// </summary>
        /// <remarks>
        /// On success, returns a 201 Created with a JWT token and expiration details.
        /// On failure, returns 400 for invalid input or 401 for unauthorized credentials.
        /// </remarks>
        /// <param name="login">The login request containing user credentials.</param>
        /// <returns>A token response with access and refresh tokens, or an error.</returns>
        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse?>), StatusCodes.Status401Unauthorized)]
        [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginResponse200Example))]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest login)
        {
            ArgumentNullException.ThrowIfNull(login, nameof(login));

            ApiResponse<LoginResponse?> response = await _authService.GenerateJwtToken(login);

            return response.ToResult();
        }

    }
}
