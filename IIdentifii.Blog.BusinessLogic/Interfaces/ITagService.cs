namespace IIdentifii.Blog.BusinessLogic
{
    public interface ITagService
    {
        Task<ApiResponse<List<Tag>>> GetTagsAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<ApiResponse<Tag>> GetTagAsync(
            Guid tagId,
            CancellationToken token);

        Task<ApiResponse<Tag>> CreateTagAsync(
            CreateTagRequest createRequest,
            Guid userGuid,
            CancellationToken token);

        Task<ApiResponse<Tag>> UpdateTagAsync(
            UpdateTagRequest updateRequest,
            Guid userGuid,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteTagAsync(
            Guid tagId,
            Guid userGuid,
            CancellationToken token);
    }
}