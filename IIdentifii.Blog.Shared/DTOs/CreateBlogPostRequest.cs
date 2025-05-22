namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the request payload for creating a new blog post.
    /// </summary>
    public record CreateBlogPostRequest
    {
        /// <summary>
        /// The title of the blog post (3–20 characters).
        /// </summary>
        [JsonPropertyName("title")]
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// The main content of the post (20–200 characters).
        /// </summary>
        [StringLength(200, MinimumLength = 20)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
