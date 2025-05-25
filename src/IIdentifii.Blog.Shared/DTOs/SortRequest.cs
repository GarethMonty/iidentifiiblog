namespace IIdentifii.Blog.Shared
{
    public record SortRequest
    {
        [JsonPropertyName("sortBy")]
        public SortByType SortBy { get; set; } = SortByType.PostedAt;

        [JsonPropertyName("sortOrder")]
        public SortOrderType SortOrder { get; set; } = SortOrderType.Descending;  

        public void Validate()
        {
        }
    }
}
