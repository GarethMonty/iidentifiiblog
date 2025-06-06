﻿namespace IIdentifii.Blog.BusinessLogic
{
    internal class CommentService : ICommentService
    {
        #region Fields

        private readonly ICommentRepository _commentRepository;

        #endregion

        #region Constructor Methods

        public CommentService(
            ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        #endregion

        #region Methods

        public async Task<PagedApiResponse<Comment>> GetCommentsAsync(
            CommentRequest request,
            CancellationToken token)
        {
            PagedResultModel<CommentModel> pagedModels = await _commentRepository.GetCommentsAsync(request, token);

            return PagedApiResponse<Comment>.Success(
                data: pagedModels.Items.Adapt<List<Comment>>(),
                page: pagedModels.Page,
                size: pagedModels.PageSize,
                totalCount: pagedModels.TotalCount);
        }

        public async Task<ApiResponse<Comment>> GetCommentAsync(
            Guid commentId,
            CancellationToken token)
        {
            CommentModel? model = await _commentRepository.GetCommentByIdAsync(commentId, token);

            if (model is null)
            {
                return ApiResponse<Comment>.NotFound($"Comment with id {commentId} not found");
            }

            return ApiResponse<Comment>.Success(model.Adapt<Comment>());
        }

        public async Task<ApiResponse<Comment>> CreateCommentAsync(
            CreateCommentRequest createRequest,
            Guid userId,
            CancellationToken token)
        {
            CommentModel model = new CommentModel()
            {
                Id = Guid.CreateVersion7(),
                BlogPostId = createRequest.BlogPostId,
                Content = createRequest.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            CommentModel createdModel = await _commentRepository.AddCommentAsync(model, token);

            return ApiResponse<Comment>.Success(createdModel.Adapt<Comment>());
        }

        public async Task<ApiResponse<Comment>> UpdateCommentAsync(
            UpdateCommentRequest updateRequest,
            Guid userId,
            CancellationToken token)
        {
            CommentModel? model = await _commentRepository.GetCommentByIdAsync(updateRequest.Id, token);

            if (model is null)
            {
                return ApiResponse<Comment>.NotFound($"Comment with id {updateRequest.Id} not found");
            }
            else if (model.UserId != userId)
            {
                return ApiResponse<Comment>.Unauthorized($"User {userId} is not authorized to update this comment");
            }

            model.Content = updateRequest.Content;

            CommentModel updatedModel = await _commentRepository.UpdateCommentAsync(model, token);

            return ApiResponse<Comment>.Success(updatedModel.Adapt<Comment>());
        }

        public async Task<ApiResponse<bool>> DeleteCommentAsync(
            Guid commentId,
            Guid userId,
            CancellationToken token)
        {
            CommentModel? model = await _commentRepository.GetCommentByIdAsync(commentId, token);

            if (model is null)
            {
                return ApiResponse<bool>.NotFound($"Comment with id {commentId} not found");
            }
            else if (model.UserId != userId)
            {
                return ApiResponse<bool>.Unauthorized($"User {userId} is not authorized to delete this comment");
            }

            bool deleted = await _commentRepository.DeleteCommentAsync(commentId, token);

            return ApiResponse<bool>.Success(deleted);
        }

        #endregion
    }
}
