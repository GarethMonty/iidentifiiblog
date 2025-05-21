using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Shared
{
    public record BlogPostRequest
    {
        [JsonPropertyName("authorId")]
        public Guid? AuthorId { get; set; }

        [JsonPropertyName("dateFilter")]
        public DateFilterRequest? DateFilter { get; set; }

        [JsonPropertyName("filter")]
        public FilterRequest? Filter { get; set; }

        [JsonPropertyName("paging")]
        public PagingRequest? Paging { get; set; }
    }
}
