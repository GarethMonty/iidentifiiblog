namespace IIdentifii.Blog.Shared
{
    public record UpdateCommentRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [StringLength(300, MinimumLength = 1)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

}
