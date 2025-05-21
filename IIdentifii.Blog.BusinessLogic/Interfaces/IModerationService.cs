namespace IIdentifii.Blog.BusinessLogic
{
    public interface IModerationService
    {
        Task<ApiResponse<List<Moderation>>> GetModerationsAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ApiResponse<Moderation>> GetModerationAsync(
            Guid moderationId,
            CancellationToken token);

        Task<ApiResponse<Moderation>> CreateModerationAsync(
            CreateModerationRequest createRequest,
            CancellationToken token);

        Task<ApiResponse<Moderation>> UpdateModerationAsync(
            UpdateModerationRequest updateRequest,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteModerationAsync(
            Guid moderationId,
            CancellationToken token);
    }
}