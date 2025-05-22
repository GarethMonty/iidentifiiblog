namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a simple text-based filter (e.g., search query).
    /// </summary>
    public record ReactionFilterRequest
    {
        /// <summary>
        /// Gets or sets a value indicating whether entities should be included in the response.
        /// </summary>
        [JsonPropertyName("includeEntities")]
        public bool IncludeEntities { get; set; } = false;

        [JsonPropertyName("excluded")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public List<ReactionType> ExcludedTypes { get; set; } = new List<ReactionType>();

        public void Validate()
        {
            //Placeholder for future validation logic
        }
    }
}
