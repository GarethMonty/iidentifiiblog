namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a user comment made on a blog post.
    /// </summary>
    public record Comment
    {
        /// <summary>
        /// Unique identifier for the comment.
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The content of the comment (1–300 characters).
        /// </summary>
        [StringLength(300, MinimumLength = 1)]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// The date and time the comment was created.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The user who made the comment.
        /// </summary>
        [JsonPropertyName("user")]
        public BlogUser User { get; set; }
    }
}
