namespace IIdentifii.Blog.Repository
{
    public interface IModerationRepository
    {
        Task<List<ModeratorFlagModel>> GetModerationsAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ModeratorFlagModel?> GetModerationByIdAsync(
            Guid moderationId,
            CancellationToken token);

        Task<ModeratorFlagModel> AddModerationAsync(
            ModeratorFlagModel moderation,
            CancellationToken token);

        Task<ModeratorFlagModel> UpdateModerationAsync(
            ModeratorFlagModel moderation,
            CancellationToken token);

        Task<bool> DeleteModerationAsync(
            Guid moderationId,
            CancellationToken token);
    }
}