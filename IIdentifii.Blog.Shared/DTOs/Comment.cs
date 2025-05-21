namespace IIdentifii.Blog.Shared
{
    public record Comment
    {
        #region Properties

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [StringLength(300, MinimumLength = 1)]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("user")]
        public BlogUser User { get; set; }

        #endregion
    }
}
