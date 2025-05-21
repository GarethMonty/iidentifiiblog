namespace IIdentifii.Blog.Repository
{
    public interface ICommentRepository
    {
        Task<List<CommentModel>> GetCommentsAsync(
            CommentRequest request,
            CancellationToken token);

        Task<CommentModel?> GetCommentByIdAsync(
            Guid commentId,
            CancellationToken token);

        Task<CommentModel> AddCommentAsync(
            CommentModel comment,
            CancellationToken token);

        Task<CommentModel> UpdateCommentAsync(
            CommentModel comment,
            CancellationToken token);

        Task<bool> DeleteCommentAsync(
            Guid commentId,
            CancellationToken token);
    }
}