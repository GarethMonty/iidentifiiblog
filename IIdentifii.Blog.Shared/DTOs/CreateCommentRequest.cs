namespace IIdentifii.Blog.Shared
{
    public record CreateCommentRequest
    {
        [Required]
        [JsonPropertyName("blogPostId")]
        public Guid BlogPostId { get; set; }

        [StringLength(300, MinimumLength = 1)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

}
