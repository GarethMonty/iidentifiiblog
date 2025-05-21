namespace IIdentifii.Blog.Shared
{
    public class Moderation
    {
        #region Properties

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("moderator")]
        public BlogUser Moderator { get; set; }

        [JsonPropertyName("flaggedAt")]
        public DateTime FlaggedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("reason")]
        public ModerationReasonType Reason { get; set; }

        #endregion
    }
}
