namespace IIdentifii.Blog.Repository
{
    public class TagModel : IDeletableEntity
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public Guid ModeratorId { get; set; }

        public IIdentifiiUser Moderator { get; set; }

        public DateTime TaggedAt { get; set; } = DateTime.UtcNow;

        public TagType Type { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedAt { get; set; }

        #endregion

        #region Methods

        public static TagModel CreateSeedTag(
            Guid id,
            Guid blogPostId,
            Guid moderatorId,
            TagType reason = TagType.None)
        {
            TagModel TagModel = new TagModel()
            {
                Id = id,
                BlogPostId = blogPostId,
                ModeratorId = moderatorId,
                Type = reason
            };

            return TagModel;
        }

        #endregion
    }
}
