namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the request to update an existing comment.
    /// </summary>
    public record UpdateCommentRequest
    {
        /// <summary>
        /// The ID of the comment to update.
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The updated content of the comment (1–300 characters).
        /// </summary>
        [StringLength(300, MinimumLength = 1)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

}
