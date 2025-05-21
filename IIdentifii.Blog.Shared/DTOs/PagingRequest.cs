namespace IIdentifii.Blog.Shared
{
    public record PagingRequest
    {
        [Range(0, int.MaxValue, MinimumIsExclusive = true, MaximumIsExclusive = false)]
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [Range(0, 100, MinimumIsExclusive = true, MaximumIsExclusive = true)]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 20;

        public void Validate()
        {
            if (Page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Page), "Page must be greater than 0.");
            }
            else if (PageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(PageSize), "PageSize must be greater than 0.");
            }
        }
    }
}
