namespace IIdentifii.Blog.BusinessLogic
{
    internal class TagService : ITagService
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly ITagRepository _tagRepository;

        #endregion

        #region Constructor Methods

        public TagService(
            IRequestContextService requestContextService,
            ITagRepository tagRepository)
        {
            _requestContextService = requestContextService;

            _tagRepository = tagRepository;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<List<Tag>>> GetTagsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            List<TagModel> models = await _tagRepository.GetTagsAsync(blogPostId, token);

            return ApiResponse<List<Tag>>.Success(models.Adapt<List<Tag>>());
        }

        public async Task<ApiResponse<Tag>> GetTagAsync(
            Guid tagId,
            CancellationToken token)
        {
            TagModel? model = await _tagRepository.GetTagByIdAsync(tagId, token);

            if (model is null)
            {
                return ApiResponse<Tag>.NotFound($"Tag with id {tagId} not found");
            }

            return ApiResponse<Tag>.Success(model.Adapt<Tag>());
        }

        public async Task<ApiResponse<Tag>> CreateTagAsync(
            CreateTagRequest createRequest,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Tag>.Failure($"User not found");
            }

            TagModel model = new TagModel()
            {
                Id = Guid.CreateVersion7(),
                BlogPostId = createRequest.BlogPostId,
                TaggedAt = DateTime.UtcNow,
                ModeratorId = userId,
                Type = createRequest.Type
            };

            TagModel createdModel = await _tagRepository.AddTagAsync(model, token);

            return ApiResponse<Tag>.Success(createdModel.Adapt<Tag>());
        }

        public async Task<ApiResponse<Tag>> UpdateTagAsync(
            UpdateTagRequest updateRequest,
            CancellationToken token)
        {
            TagModel? model = await _tagRepository.GetTagByIdAsync(updateRequest.Id, token);

            if (model is null)
            {
                return ApiResponse<Tag>.NotFound($"Tag with id {updateRequest.Id} not found");
            }

            model.Type = updateRequest.Type;

            TagModel updatedModel = await _tagRepository.UpdateTagAsync(model, token);

            return ApiResponse<Tag>.Success(updatedModel.Adapt<Tag>());
        }

        public async Task<ApiResponse<bool>> DeleteTagAsync(
            Guid tagId,
            CancellationToken token)
        {
            TagModel? model = await _tagRepository.GetTagByIdAsync(tagId, token);

            if (model is null)
            {
                return ApiResponse<bool>.NotFound($"Tag with id {tagId} not found");
            }

            bool deleted = await _tagRepository.DeleteTagAsync(tagId, token);

            return ApiResponse<bool>.Success(deleted);
        }

        #endregion
    }
}
