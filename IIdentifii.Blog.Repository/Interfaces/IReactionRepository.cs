namespace IIdentifii.Blog.Repository
{
    public interface IReactionRepository
    {
        Task<List<ReactionModel>> GetReactionsAsync(
            Guid blogPostId, 
            CancellationToken token);

        Task<List<ReactionModel>> GetReactionsByTypeAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token);

        Task<int> GetReactionCountAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token);

        Task<ReactionModel?> GetReactionByIdAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token);

        Task<ReactionModel> AddReactionAsync(
            ReactionModel reaction,
            CancellationToken token);

        Task<bool> DeleteReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token);
    }
}