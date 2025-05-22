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
            CancellationToken token);

        Task<ApiResponse<Tag>> UpdateTagAsync(
            UpdateTagRequest updateRequest,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteTagAsync(
            Guid tagId,
            CancellationToken token);
    }
}