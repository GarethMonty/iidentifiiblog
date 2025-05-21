using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Shared
{
    public record PagingRequest
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 20;
    }
}
