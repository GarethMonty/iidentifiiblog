namespace IIdentifii.Blog.BusinessLogic
{
    internal class TagService : ITagService
    {
        #region Fields

        private readonly ITagRepository _tagRepository;

        #endregion

        #region Constructor Methods

        public TagService(
            ITagRepository tagRepository)
        {
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
            Guid userId,
            CancellationToken token)
        {
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
            Guid userId,
            CancellationToken token)
        {
            TagModel? model = await _tagRepository.GetTagByIdAsync(updateRequest.Id, token);

            if (model is null)
            {
                return ApiResponse<Tag>.NotFound($"Tag with id {updateRequest.Id} not found");
            }
            else if (model.ModeratorId != userId)
            {
                return ApiResponse<Tag>.Unauthorized($"Moderator {userId} is not authorized to update this tag");
            }

            model.Type = updateRequest.Type;

            TagModel updatedModel = await _tagRepository.UpdateTagAsync(model, token);

            return ApiResponse<Tag>.Success(updatedModel.Adapt<Tag>());
        }

        public async Task<ApiResponse<bool>> DeleteTagAsync(
            Guid tagId,
            Guid userId,
            CancellationToken token)
        {
            TagModel? model = await _tagRepository.GetTagByIdAsync(tagId, token);

            if (model is null)
            {
                return ApiResponse<bool>.NotFound($"Tag with id {tagId} not found");
            }
            else if (model.ModeratorId != userId)
            {
                return ApiResponse<bool>.Unauthorized($"Moderator {userId} is not authorized to delete this tag");
            }

            bool deleted = await _tagRepository.DeleteTagAsync(tagId, token);

            return ApiResponse<bool>.Success(deleted);
        }

        #endregion
    }
}
