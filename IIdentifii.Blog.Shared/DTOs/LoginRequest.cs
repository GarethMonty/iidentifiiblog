namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a login request with email and password.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}
