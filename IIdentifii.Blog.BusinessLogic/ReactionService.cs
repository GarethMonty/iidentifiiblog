namespace IIdentifii.Blog.BusinessLogic
{
    internal class ReactionService : IReactionService
    {
        #region Fields

        private readonly IReactionHandler _reactionHandler;

        private readonly IReactionRepository _likeRepository;

        #endregion

        #region Constructor Methods

        public ReactionService(
            IReactionHandler reactionHandler,
            IReactionRepository likeRepository)
        {
            _reactionHandler = reactionHandler;

            _likeRepository = likeRepository;
        }

        #endregion

        #region Methods

        public Task<ApiResponse<List<Reaction>>> GetLikeReactionsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return GetReactionsAsync(blogPostId, ReactionType.Like, token);
        }

        public async Task<ApiResponse<List<Reaction>>> GetReactionsAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            List<ReactionModel> models = await _likeRepository.GetReactionsByTypeAsync(blogPostId, type, token);

            return ApiResponse<List<Reaction>>.Success(models.Adapt<List<Reaction>>());
        }

        public async Task<ApiResponse<int>> GetReactionCountAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            int count = await _likeRepository.GetReactionCountAsync(blogPostId, type, token);

            return ApiResponse<int>.Success(count);
        }

        public async Task<ApiResponse<Reaction>> GetReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token)
        {
            ReactionModel? model = await _likeRepository.GetReactionByIdAsync(blogPostId, userId, type, token);

            if (model is null)
            {
                return ApiResponse<Reaction>.NotFound($"Reaction [{type}] for blog post [{blogPostId}] and user [{userId}] not found");
            }

            return ApiResponse<Reaction>.Success(model.Adapt<Reaction>());
        }

        public async Task<ApiResponse<Reaction>> CreateReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token)
        {
            ReactionModel model = new ReactionModel()
            {
                Id = Guid.CreateVersion7(),
                BlogPostId = blogPostId,
                UserId = userId,
                Type = type,
                ReactedAt = DateTime.UtcNow
            };

            ReactionModel createdModel = await _likeRepository.AddReactionAsync(model, token);

            return ApiResponse<Reaction>.Success(createdModel.Adapt<Reaction>());
        }

        public async Task<ApiResponse<Reaction>> ChangeReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            ReactionType previousType,
            CancellationToken token)
        {
            ReactionModel? model = await _likeRepository.GetReactionByIdAsync(blogPostId, userId, previousType, token);

            if (model is null)
            {
                return ApiResponse<Reaction>.NotFound($"Reaction [{previousType}] for blog post [{blogPostId}] and user [{userId}] not found");
            }
            else if (model.UserId != userId)
            {
                return ApiResponse<Reaction>.Unauthorized($"User {userId} is not authorized to update this reaction");
            }

            model.Type = type;

            ReactionModel createdModel = await _likeRepository.UpdateReactionAsync(model, token);

            return ApiResponse<Reaction>.Success(createdModel.Adapt<Reaction>());
        }

        public async Task<ApiResponse<bool>> DeleteReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token)
        {
            ReactionModel? model = await _likeRepository.GetReactionByIdAsync(blogPostId, userId, type, token);

            if (model is null)
            {
                return ApiResponse<bool>.NotFound($"Reaction with for blog post [{blogPostId}] and user [{userId}] not found");
            }
            else if (model.UserId != userId)
            {
                return ApiResponse<bool>.Unauthorized($"User {userId} is not authorized to delete this reaction");
            }

            bool deleted = await _likeRepository.DeleteReactionAsync(blogPostId, userId, type, token);

            return ApiResponse<bool>.Success(deleted);
        }

        #endregion
    }
}
