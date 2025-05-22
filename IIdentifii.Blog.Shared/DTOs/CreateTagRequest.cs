namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the request to flag a blog post for tag.
    /// </summary>
    public record CreateTagRequest
    {
        /// <summary>
        /// The ID of the blog post being tagged.
        /// </summary>
        [Required]
        [JsonPropertyName("blogPostId")]
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// The reason for tag.
        /// </summary>
        [JsonPropertyName("type")]
        public TagType Type { get; set; }
    }
}
