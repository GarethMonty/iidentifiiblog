namespace IIdentifii.Blog.BusinessLogic
{
    public interface IReactionService
    {
        Task<ApiResponse<List<Reaction>>> GetLikeReactionsAsync(
            Guid blogPostId, 
            CancellationToken token);

        Task<ApiResponse<List<Reaction>>> GetReactionsAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token);

        Task<ApiResponse<int>> GetReactionCountAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token);

        Task<ApiResponse<Reaction>> CreateReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token);

        Task<ApiResponse<Reaction>> GetReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token);
    }
}