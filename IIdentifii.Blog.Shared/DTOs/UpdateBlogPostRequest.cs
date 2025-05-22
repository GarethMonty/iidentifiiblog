namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the request to update an existing blog post.
    /// </summary>
    public record UpdateBlogPostRequest
    {
        /// <summary>
        /// The ID of the blog post to update.
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The new title of the blog post (3–20 characters).
        /// </summary>
        [JsonPropertyName("title")]
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// The updated content of the post (20–200 characters).
        /// </summary>
        [StringLength(200, MinimumLength = 20)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
