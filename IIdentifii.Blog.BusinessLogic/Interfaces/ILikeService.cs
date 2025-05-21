namespace IIdentifii.Blog.BusinessLogic
{
    public interface ILikeService
    {
        Task<ApiResponse<List<Like>>> GetLikesAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ApiResponse<int>> GetLikeCountAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ApiResponse<Like>> CreateLikeAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ApiResponse<Like>> GetLikeAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteLikeAsync(
            Guid blogPostId,
            CancellationToken token);
    }
}