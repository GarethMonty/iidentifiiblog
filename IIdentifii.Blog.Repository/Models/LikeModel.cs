namespace IIdentifii.Blog.Repository
{
    public record LikeModel : IDeletableEntity
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public Guid UserId { get; set; }

        public IIdentifiiUser User { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public DateTime DeletedAt { get; set; }

        #endregion
    }
}
