namespace IIdentifii.Blog.Shared
{
    public record FilterRequest
    {
        [MaxLength(50)]
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        public void Validate()
        {
        }
    }
}
