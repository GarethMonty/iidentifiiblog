namespace IIdentifii.Blog.Shared
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiration { get; set; }

        #region Static Methods

        public static LoginResponse Create(
            string token,
            DateTime expiration, 
            string refreshToken, 
            DateTime refreshTokenExpiration)
        {
            return new LoginResponse()
            {
                Token = token,
                Expiration = expiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }
        #endregion
    }
}
