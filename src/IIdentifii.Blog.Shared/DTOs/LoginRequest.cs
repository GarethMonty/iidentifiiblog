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
        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(300)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
