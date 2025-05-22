namespace IIdentifii.Blog.Repository
{
    public interface IReactionAggregateRepository
    {
        Task<List<ReactionAggregateModel>> GetReactionsAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ReactionAggregateModel?> GetReactionAggregateByIdAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token);

        Task<ReactionAggregateModel> AddReactionAggregateAsync(
            ReactionAggregateModel reactionAggregate,
            CancellationToken token);

        int? AddReactionUpdate(
            ReactionAggregateModel reactionAggregateModel);

        Task SaveReactionChangesAsync(
            CancellationToken token);

        Task<bool> DeleteReactionAggregateAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token);
    }
}