namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a request to filter blog posts by a collection of tag identifiers.
    /// </summary>
    public record TagFilterRequest
    {
        /// <summary>
        /// A list of tag types to filter the blog posts by.
        /// </summary>
        [JsonPropertyName("tags")]
        public List<TagType> Tags { get; set; } = new List<TagType>();

        public void Validate()
        {
            // Placeholder for future validation logic
        }
    }
}
