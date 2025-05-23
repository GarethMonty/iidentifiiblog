namespace IIdentifii.Blog.Repository
{
    public record CommentModel : ISoftDelete
    {
        #region Properties

        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset DeletedAt { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public Guid UserId { get; set; }

        public IIdentifiiUser User { get; set; }

        #endregion

        #region Methods

        public static CommentModel CreateSeedComment(
            Guid id,
            Guid userId,
            Guid blogPostId,
            string content = "This is a sample comment content.")
        {
            CommentModel commentModel = new CommentModel()
            {
                Id = id,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                BlogPostId = blogPostId
            };
            return commentModel;
        }

        #endregion
    }
}
