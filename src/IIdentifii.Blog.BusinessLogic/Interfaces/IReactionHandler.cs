namespace IIdentifii.Blog.BusinessLogic
{
    public interface IReactionHandler
    {
        Task<ReactionOperation?> DequeueReactionAsync(CancellationToken token);
        Task QueueReactionAsync(ReactionOperation reactionModel, CancellationToken token);
    }
}