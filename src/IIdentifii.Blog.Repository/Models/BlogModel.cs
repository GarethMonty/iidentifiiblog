namespace IIdentifii.Blog.Repository
{
    public record BlogPostModel : ISoftDelete
    {
        #region Properties

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset DeletedAt { get; set; }

        public Guid AuthorId { get; set; }

        public IIdentifiiUser Author { get; set; }

        public ICollection<ReactionModel> Reactions { get; set; } = new List<ReactionModel>();

        public ICollection<ReactionAggregateModel> ReactionAggregates { get; set; } = new List<ReactionAggregateModel>();

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();

        public ICollection<TagModel> Tags { get; set; } = new List<TagModel>();

        #endregion

        #region Methods

        public void SetupForCreate(
            Guid userId)
        {
            Id = Guid.CreateVersion7();
            AuthorId = userId;
            PostedAt = DateTime.UtcNow;

            foreach (ReactionType reactionType in Enum.GetValues<ReactionType>())
            {
                ReactionAggregates.Add(new ReactionAggregateModel()
                {
                    Id = Guid.CreateVersion7(),
                    BlogPostId = Id,
                    Type = reactionType,
                    Count = 0
                });
            }

        }

        internal static BlogPostModel CreateSeedPost(
            Guid id,
            Guid userId,
            string title = "Sample Blog Post",
            string content = "This is a sample blog post content.",
            List<ReactionModel>? reactionModels = null,
            List<ReactionAggregateModel>? reactionAggregateModels = null,
            List<CommentModel>? commentModels = null,
            List<TagModel>? tagModels = null,
            bool isDeleted = false)
        {
            BlogPostModel blogPostModel = new BlogPostModel()
            {
                Id = id,
                Title = title,
                Content = content,
                PostedAt = DateTime.UtcNow,
                AuthorId = userId,
                Reactions = reactionModels ?? new List<ReactionModel>(),
                ReactionAggregates = reactionAggregateModels ?? new List<ReactionAggregateModel>(),
                Comments = commentModels ?? new List<CommentModel>(),
                Tags = tagModels ?? new List<TagModel>(),
                IsDeleted = isDeleted,
            };

            return blogPostModel;
        }

        #endregion
    }
}
