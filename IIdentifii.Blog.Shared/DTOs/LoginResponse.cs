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
        [JsonPropertyName("validTo")]
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Helper method for creating a login response.
        /// </summary>
        public static LoginResponse Create(
            string token,
            DateTime validTo)
        {
            return new LoginResponse()
            {
                Token = token,
                ValidTo = validTo,
            };
        }
    }
}
