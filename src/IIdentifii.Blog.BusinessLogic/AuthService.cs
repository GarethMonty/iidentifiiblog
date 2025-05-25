namespace IIdentifii.Blog.BusinessLogic
{
    internal class AuthService : IAuthService
    {
        #region Fields

        private readonly AuthSettings _settings;

        private readonly UserManager<IIdentifiiUser> _userManager;

        #endregion

        #region Constructor Methods

        public AuthService(
            IOptions<AuthSettings> options,
            UserManager<IIdentifiiUser> userManager)
        {
            _settings = options.Value;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<LoginResponse?>> GenerateJwtToken(
            LoginRequest request)
        {
            IIdentifiiUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return ApiResponse<LoginResponse?>.Unauthorized("Invalid credentials");
            }

            List<Claim> authClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(ClaimTypes.Email, user.Email ?? "")
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);

            authClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return ApiResponse<LoginResponse?>.Success(LoginResponse.Create(token, jwtToken.ValidTo));
        }

        #endregion
    }
}
