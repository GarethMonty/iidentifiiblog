using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Shared
{
    public record DateFilterRequest
    {
        [JsonPropertyName("from")]
        public DateTime? From { get; set; }

        [JsonPropertyName("to")]
        public DateTime? To { get; set; }
    }
}
