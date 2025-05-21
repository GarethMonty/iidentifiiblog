namespace IIdentifii.Blog.Shared
{
    public record BlogUser
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("bio")]
        public string? Bio { get; set; }

    }
}
