namespace IIdentifii.Blog.Shared
{
    public record CreateBlogPostRequest
    {
        [JsonPropertyName("title")]
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(200, MinimumLength = 20)]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

}
