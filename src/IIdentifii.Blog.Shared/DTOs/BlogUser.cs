namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a user who can author blog posts and comments.
    /// </summary>
    public record BlogUser
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The display name of the user (shown publicly).
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }
}
