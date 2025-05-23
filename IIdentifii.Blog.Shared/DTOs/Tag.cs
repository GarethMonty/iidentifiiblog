namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a tag record applied to a blog post.
    /// </summary>
    public class Tag
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The moderator who tagged the post.
        /// </summary>
        [JsonPropertyName("moderator")]
        public BlogUser Moderator { get; set; }

        /// <summary>
        /// The date and time the post was tagged.
        /// </summary>
        [JsonPropertyName("flaggedAt")]
        public DateTime FlaggedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The reason the post was tagged.
        /// </summary>
        [JsonPropertyName("type")]
        public TagType Type { get; set; }
    }
}
