namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the request payload for creating a new comment.
    /// </summary>
    public record CreateCommentRequest
    {
        /// <summary>
        /// The ID of the blog post the comment is associated with.
        /// </summary>
        [Required]
        [JsonPropertyName("blogPostId")]
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// The content of the comment (1–300 characters).
        /// </summary>
        [StringLength(300, MinimumLength = 1)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
