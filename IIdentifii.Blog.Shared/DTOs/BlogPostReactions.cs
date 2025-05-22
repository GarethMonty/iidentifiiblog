namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents the reactions to a blog post, including counts and details of each reaction.
    /// </summary>
    public record BlogPostReactions
    {
        /// <summary>
        /// Gets or sets the counts of reactions, grouped by reaction type.
        /// </summary>
        [JsonPropertyName("counts")]
        public Dictionary<ReactionType, int> Counts { get; set; } = new();

        /// <summary>
        /// Gets or sets the details of each reaction, including the user who reacted and the time of reaction.
        /// </summary>
        [JsonPropertyName("entities")]
        public Dictionary<ReactionType, Reaction> Entities { get; set; } = new();
    }
}
