namespace IIdentifii.Blog.Shared
{
    public record CommentRequest
    {
        [JsonPropertyName("blogPostId")]
        public Guid? BlogPostId { get; set; }

        [JsonPropertyName("userId")]
        public Guid? UserId { get; set; }

        [JsonPropertyName("dateFilter")]
        public DateFilterRequest? DateFilter { get; set; }

        [JsonPropertyName("filter")]
        public FilterRequest? Filter { get; set; }

        [JsonPropertyName("paging")]
        public PagingRequest? Paging { get; set; }

        public void Validate()
        {
            DateFilter?.Validate();
            Filter?.Validate();
            Paging?.Validate();
        }
    }
}
