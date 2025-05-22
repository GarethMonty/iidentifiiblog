namespace IIdentifii.Blog.Repository
{
    public record ReactionModel : IDeletableEntity
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public ReactionType Type { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public Guid UserId { get; set; }

        public IIdentifiiUser User { get; set; }

        public DateTime ReactedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public DateTime DeletedAt { get; set; }

        #endregion

        #region Methods

        public static ReactionModel CreateSeedReaction(
            Guid id,
            Guid blogPostId,
            Guid userId,
            ReactionType type = ReactionType.Like,
            DateTime? reactedAt = null)
        {
            ReactionModel reactionModel = new ReactionModel()
            {
                Id = id,
                BlogPostId = blogPostId,
                UserId = userId,
                Type = type,
                ReactedAt = reactedAt ?? DateTime.UtcNow
            };
            return reactionModel;
        }

        #endregion
    }
}
