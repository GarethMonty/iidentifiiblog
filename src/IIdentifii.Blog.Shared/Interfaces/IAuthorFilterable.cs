namespace IIdentifii.Blog.Shared
{
    public interface IAuthorFilterable : IFilterable
    {
        /// <summary>
        /// Filter by a specific author's ID.
        /// </summary>
        Guid? AuthorId { get; set; }
    }

    public interface IUserFilterable : IFilterable
    {
        /// <summary>
        /// Filter by a specific user's ID.
        /// </summary>
        Guid? UserId { get; set; }
    }

    public interface IModeratorFilterable : IFilterable
    {
        /// <summary>
        /// Filter by a specific moderator's ID.
        /// </summary>
        Guid? ModeratorId { get; set; }
    }

}
