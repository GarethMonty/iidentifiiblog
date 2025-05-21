namespace IIdentifii.Blog.Repository
{
    public class ModeratorFlagModel : IDeletableEntity
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public Guid ModeratorId { get; set; }

        public IIdentifiiUser Moderator { get; set; }

        public DateTime FlaggedAt { get; set; } = DateTime.UtcNow;

        public ModerationReasonType Reason { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedAt { get; set; }

        #endregion
    }
}
