namespace IIdentifii.Blog.Shared
{
    public record BlogPost
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(200, MinimumLength = 20)]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        [JsonPropertyName("author")]
        public BlogUser Author { get; set; }

        [JsonPropertyName("likes")]
        public List<Like> Likes { get; set; } = new List<Like>();

        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; } = new List<Comment>();

        [JsonPropertyName("moderations")]
        public List<Moderation> Moderations { get; set; } = new List<Moderation>();
    }
}
