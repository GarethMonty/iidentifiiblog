namespace IIdentifii.Blog.Repository
{
    public record BlogPostModel : IDeletableEntity
    {
        #region Properties

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedAt { get; set; }

        public Guid AuthorId { get; set; }

        public IIdentifiiUser Author { get; set; }

        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();

        public ICollection<ModeratorFlagModel> ModeratorFlags { get; set; } = new List<ModeratorFlagModel>();

        #endregion
    }
}
