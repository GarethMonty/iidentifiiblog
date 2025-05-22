namespace IIdentifii.Blog.BusinessLogic
{
    public interface ICommentService
    {
        Task<ApiResponse<List<Comment>>> GetCommentsAsync(
            CommentRequest request,
            CancellationToken token);

        Task<ApiResponse<Comment>> GetCommentAsync(
            Guid commentId,
            CancellationToken token);

        Task<ApiResponse<Comment>> CreateCommentAsync(
            CreateCommentRequest createRequest,
            Guid userId,
            CancellationToken token);

        Task<ApiResponse<Comment>> UpdateCommentAsync(
            UpdateCommentRequest updateRequest,
            Guid userId,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteCommentAsync(
            Guid commentId,
            Guid userId,
            CancellationToken token);
    }
}