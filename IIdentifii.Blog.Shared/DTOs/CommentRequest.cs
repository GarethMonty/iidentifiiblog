namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// A request object for filtering and retrieving comments.
    /// </summary>
    public record CommentRequest
    {
        /// <summary>
        /// Optional filter for comments on a specific blog post.
        /// </summary>
        [JsonPropertyName("blogPostId")]
        public Guid? BlogPostId { get; set; }

        /// <summary>
        /// Optional filter for comments made by a specific user.
        /// </summary>
        [JsonPropertyName("userId")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// Optional filter for date ranges.
        /// </summary>
        [JsonPropertyName("dateFilter")]
        public DateFilterRequest? DateFilter { get; set; }

        /// <summary>
        /// Optional content/text filters.
        /// </summary>
        [JsonPropertyName("filter")]
        public FilterRequest? Filter { get; set; }

        /// <summary>
        /// Paging parameters for the comment list.
        /// </summary>
        [JsonPropertyName("paging")]
        public PagingRequest? Paging { get; set; }

        /// <summary>
        /// Validates all nested request filters and paging.
        /// </summary>
        public void Validate()
        {
            DateFilter?.Validate();
            Filter?.Validate();
            Paging?.Validate();
        }
    }
}
