using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Shared
{
    public record FilterRequest
    {
        [JsonPropertyName("query")]
        public string? Query { get; set; }
    }
}
