namespace IIdentifii.Blog.BusinessLogic
{
    internal class ModerationService : IModerationService
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly IModerationRepository _moderationRepository;

        #endregion

        #region Constructor Methods

        public ModerationService(
            IRequestContextService requestContextService,
            IModerationRepository moderationRepository)
        {
            _requestContextService = requestContextService;

            _moderationRepository = moderationRepository;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<List<Moderation>>> GetModerationsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            List<ModeratorFlagModel> models = await _moderationRepository.GetModerationsAsync(blogPostId, token);

            return ApiResponse<List<Moderation>>.Success(models.Adapt<List<Moderation>>());
        }

        public async Task<ApiResponse<Moderation>> GetModerationAsync(
            Guid moderationId,
            CancellationToken token)
        {
            ModeratorFlagModel? model = await _moderationRepository.GetModerationByIdAsync(moderationId, token);

            if (model is null)
            {
                return ApiResponse<Moderation>.NotFound($"Moderation with id {moderationId} not found");
            }

            return ApiResponse<Moderation>.Success(model.Adapt<Moderation>());
        }

        public async Task<ApiResponse<Moderation>> CreateModerationAsync(
            CreateModerationRequest createRequest,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Moderation>.Failure($"User not found");
            }

            ModeratorFlagModel model = new ModeratorFlagModel()
            {
                Id = Guid.CreateVersion7(),
                BlogPostId = createRequest.BlogPostId,
                FlaggedAt = DateTime.UtcNow,
                ModeratorId = userId,
                Reason = createRequest.Reason
            };

            ModeratorFlagModel createdModel = await _moderationRepository.AddModerationAsync(model, token);

            return ApiResponse<Moderation>.Success(createdModel.Adapt<Moderation>());
        }

        public async Task<ApiResponse<Moderation>> UpdateModerationAsync(
            UpdateModerationRequest updateRequest,
            CancellationToken token)
        {
            ModeratorFlagModel? model = await _moderationRepository.GetModerationByIdAsync(updateRequest.Id, token);

            if (model is null)
            {
                return ApiResponse<Moderation>.NotFound($"Moderation with id {updateRequest.Id} not found");
            }

            model.Reason = updateRequest.Reason;

            ModeratorFlagModel updatedModel = await _moderationRepository.UpdateModerationAsync(model, token);

            return ApiResponse<Moderation>.Success(updatedModel.Adapt<Moderation>());
        }

        public async Task<ApiResponse<bool>> DeleteModerationAsync(
            Guid moderationId,
            CancellationToken token)
        {
            ModeratorFlagModel? model = await _moderationRepository.GetModerationByIdAsync(moderationId, token);

            if (model is null)
            {
                return ApiResponse<bool>.NotFound($"Moderation with id {moderationId} not found");
            }

            bool deleted = await _moderationRepository.DeleteModerationAsync(moderationId, token);

            return ApiResponse<bool>.Success(deleted);
        }

        #endregion
    }
}
