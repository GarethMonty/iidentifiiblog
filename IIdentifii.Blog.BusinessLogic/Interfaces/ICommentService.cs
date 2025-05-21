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
            CancellationToken token);

        Task<ApiResponse<Comment>> UpdateCommentAsync(
            UpdateCommentRequest updateRequest,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteCommentAsync(
            Guid commentId,
            CancellationToken token);
    }
}