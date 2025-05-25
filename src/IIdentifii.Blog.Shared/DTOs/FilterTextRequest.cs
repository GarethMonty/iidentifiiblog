namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a simple text-based filter (e.g., search query).
    /// </summary>
    public record FilterTextRequest
    {
        /// <summary>
        /// A keyword or search query used to filter results.
        /// </summary>
        [MaxLength(50)]
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        public void Validate()
        {
            //Placeholder for future validation logic if needed.
        }
    }
}
