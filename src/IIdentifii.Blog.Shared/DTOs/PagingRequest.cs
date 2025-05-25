namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents paging options for a paginated query.
    /// </summary>
    public record PagingRequest
    {
        /// <summary>
        /// The page number (starting from 1).
        /// </summary>
        [Range(0, int.MaxValue, MinimumIsExclusive = true, MaximumIsExclusive = false)]
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of items to return per page (1–100).
        /// </summary>
        [Range(0, 100, MinimumIsExclusive = true, MaximumIsExclusive = true)]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Validates page and pageSize are within acceptable ranges.
        /// </summary>
        public void Validate()
        {
            if (Page <= 0)
            {
                throw IIdentifiiException.Bad("Page must be greater than 0.");
            }
            else if (PageSize <= 0)
            {
                throw IIdentifiiException.Bad("PageSize must be greater than 0.");
            }
        }
    }
}
