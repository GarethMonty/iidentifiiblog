namespace IIdentifii.Blog.Shared
{
    public record CreateModerationRequest
    {
        [Required]
        [JsonPropertyName("blogPostId")]
        public Guid BlogPostId { get; set; }

        [JsonPropertyName("reason")]
        public ModerationReasonType Reason { get; set; }
    }
}
