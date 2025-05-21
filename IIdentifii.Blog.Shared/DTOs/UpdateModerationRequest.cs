namespace IIdentifii.Blog.Shared
{
    public record UpdateModerationRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("reason")]
        public ModerationReasonType Reason { get; set; }
    }
}
