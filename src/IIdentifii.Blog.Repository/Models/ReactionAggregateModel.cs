namespace IIdentifii.Blog.Repository
{
    public record ReactionAggregateModel : ISoftDelete
    {
        #region Properties

        public Guid Id { get; set; }

        public ReactionType Type { get; set; }

        public int Count { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset DeletedAt { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; }


        #endregion

        #region Methods

        public static ReactionAggregateModel CreateSeedReaction(
            Guid id,
            Guid blogPostId,
            ReactionType type = ReactionType.Like,
            int count = 0)
        {
            ReactionAggregateModel reactionAggregateModel = new ReactionAggregateModel()
            {
                Id = id,
                BlogPostId = blogPostId,
                Type = type,
                Count = count
            };
            return reactionAggregateModel;
        }

        #endregion
    }
}
