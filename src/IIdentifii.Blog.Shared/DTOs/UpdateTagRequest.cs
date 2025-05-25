namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the request to update a tag entry.
    /// </summary>
    public record UpdateTagRequest
    {
        /// <summary>
        /// The ID of the tag entry to update.
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The updated tag type.
        /// </summary>
        [JsonPropertyName("type")]
        public TagType Type { get; set; }
    }
}
