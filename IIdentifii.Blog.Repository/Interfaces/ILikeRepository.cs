namespace IIdentifii.Blog.Repository
{
    public interface ILikeRepository
    {
        Task<List<LikeModel>> GetLikesAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<int> GetLikeCountAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<LikeModel?> GetLikeByIdAsync(
            Guid blogPostId,
            Guid userId,
            CancellationToken token);

        Task<LikeModel> AddLikeAsync(
            LikeModel like,
            CancellationToken token);

        Task<bool> DeleteLikeAsync(
            Guid blogPostId,
            Guid userId,
            CancellationToken token);
    }
}