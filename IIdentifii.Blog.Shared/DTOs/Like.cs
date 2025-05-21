namespace IIdentifii.Blog.Shared
{
    public record Like
    {
        #region Properties

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("user")]
        public BlogUser User { get; set; }

        [JsonPropertyName("likedAt")]
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }
}
