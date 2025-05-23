namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a filter request to include or exclude specific reaction types when querying blog posts.
    /// </summary>
    public record ReactionFilterRequest
    {
        /// <summary>
        /// Indicates whether to include the full reaction entities in the response.
        /// </summary>
        [JsonPropertyName("includeEntities")]
        public bool IncludeEntities { get; set; } = false;

        /// <summary>
        /// A list of reaction types to exclude from the results.
        /// </summary>
        [JsonPropertyName("excluded")]
        public List<ReactionType> ExcludedTypes { get; set; } = new List<ReactionType>();

        public void Validate()
        {
            // Placeholder for future validation logic
        }
    }
}
