namespace IIdentifii.Blog.BusinessLogic
{
    internal class LikeService : ILikeService
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly ILikeRepository _likeRepository;

        #endregion

        #region Constructor Methods

        public LikeService(
            IRequestContextService requestContextService,
            ILikeRepository likeRepository)
        {
            _requestContextService = requestContextService;

            _likeRepository = likeRepository;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<List<Like>>> GetLikesAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            List<LikeModel> models = await _likeRepository.GetLikesAsync(blogPostId, token);

            return ApiResponse<List<Like>>.Success(models.Adapt<List<Like>>());
        }

        public async Task<ApiResponse<int>> GetLikeCountAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            int count = await _likeRepository.GetLikeCountAsync(blogPostId, token);

            return ApiResponse<int>.Success(count);
        }

        public async Task<ApiResponse<Like>> GetLikeAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Like>.Failure($"User not found");
            }

            LikeModel? model = await _likeRepository.GetLikeByIdAsync(blogPostId, userId, token);

            if (model is null)
            {
                return ApiResponse<Like>.NotFound($"Like with for blog post [{blogPostId}] and user [{userId}] not found");
            }

            return ApiResponse<Like>.Success(model.Adapt<Like>());
        }

        public async Task<ApiResponse<Like>> CreateLikeAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Like>.Failure($"User not found");
            }

            LikeModel model = new LikeModel()
            {
                Id = Guid.CreateVersion7(),
                BlogPostId = blogPostId,
                UserId = userId,
                LikedAt = DateTime.UtcNow
            };

            LikeModel createdModel = await _likeRepository.AddLikeAsync(model, token);

            return ApiResponse<Like>.Success(createdModel.Adapt<Like>());
        }

        public async Task<ApiResponse<bool>> DeleteLikeAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<bool>.Failure($"User not found");
            }

            LikeModel? model = await _likeRepository.GetLikeByIdAsync(blogPostId, userId, token);

            if (model is null)
            {
                return ApiResponse<bool>.NotFound($"Like with for blog post [{blogPostId}] and user [{userId}] not found");
            }

            bool deleted = await _likeRepository.DeleteLikeAsync(blogPostId, userId, token);

            return ApiResponse<bool>.Success(deleted);
        }

        #endregion
    }
}
