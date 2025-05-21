namespace IIdentifii.Blog.Repository
{
    public record CommentModel : IDeletableEntity
    {
        #region Properties

        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedAt { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public Guid UserId { get; set; }

        public IIdentifiiUser User { get; set; }

        #endregion
    }
}
