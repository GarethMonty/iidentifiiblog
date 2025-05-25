namespace IIdentifii.Blog.Shared
{
    public interface IReactionFilterable : IFilterable
    {
        /// <summary>
        /// Optional filter for reactions (e.g. likes, dislikes).
        /// </summary>
        ReactionFilterRequest? ReactionFilter { get; set; }
    }
}
