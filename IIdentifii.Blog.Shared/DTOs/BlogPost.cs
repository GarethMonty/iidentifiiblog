namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a blog post with author, content, and metadata.
    /// </summary>
    public record BlogPost
    {
        /// <summary>
        /// The unique identifier for the blog post.
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The title of the blog post (3–20 characters).
        /// </summary>
        [JsonPropertyName("title")]
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// The main content of the blog post (20–200 characters).
        /// </summary>
        [StringLength(200, MinimumLength = 20)]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// The date and time the post was created or published.
        /// </summary>
        [JsonPropertyName("postedAt")]
        public DateTime PostedAt { get; set; }

        /// <summary>
        /// The user who authored the blog post.
        /// </summary>
        [JsonPropertyName("author")]
        public BlogUser Author { get; set; }

        /// <summary>
        /// The list of users who have liked the post.
        /// </summary>
        [JsonPropertyName("likes")]
        public List<Reaction> Reactions { get; set; } = new();

        /// <summary>
        /// Comments made on the blog post.
        /// </summary>
        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; } = new();

        /// <summary>
        /// Moderator flags or tag actions taken on the post.
        /// </summary>
        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; } = new();
    }
}
