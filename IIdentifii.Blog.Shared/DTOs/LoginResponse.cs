namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a response from a successful login, including tokens.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// The JWT access token.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// When the access token expires.
        /// </summary>
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

        /// <summary>
        /// The refresh token used to get a new access token.
        /// </summary>
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// When the refresh token expires.
        /// </summary>
        [JsonPropertyName("refreshTokenExpiration")]
        public DateTime RefreshTokenExpiration { get; set; }

        /// <summary>
        /// Helper method for creating a login response.
        /// </summary>
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
    }
}
