namespace IIdentifii.Blog.Shared
{
    public record SortRequest
    {
        [JsonPropertyName("sortBy")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortByType SortBy { get; set; } = SortByType.PostedAt;

        [JsonPropertyName("sortOrder")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortOrderType SortOrder { get; set; } = SortOrderType.Descending;  

        public void Validate()
        {
        }
    }
}
