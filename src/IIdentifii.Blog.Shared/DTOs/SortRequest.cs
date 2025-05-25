namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a request for sorting blog posts based on specified criteria.
    /// </summary>
    public record SortRequest
    {
        /// <summary>
        /// The criteria by which to sort the blog posts.
        /// </summary>
        [JsonPropertyName("sortBy")]
        public SortByType SortBy { get; set; } = SortByType.PostedAt;

        /// <summary>
        /// The order in which to sort the blog posts (ascending or descending).
        /// </summary>
        [JsonPropertyName("sortOrder")]
        public SortOrderType SortOrder { get; set; } = SortOrderType.Descending;  

        public void Validate()
        {
        }
    }
}
