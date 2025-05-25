namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a 'reaction' action by a user on a blog post.
    /// </summary>
    public record Reaction
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The enum type of reaction (like, dislike, etc.).
        /// </summary>
        [JsonPropertyName("type")]
        public ReactionType Type { get; set; }

        /// <summary>
        /// The user who liked the post.
        /// </summary>
        [JsonPropertyName("user")]
        public BlogUser User { get; set; }

        /// <summary>
        /// The date and time the reaction was added.
        /// </summary>
        [JsonPropertyName("reactedAt")]
        public DateTime ReactedAt { get; set; } = DateTime.UtcNow;
    }
}
