namespace IIdentifii.Blog.Repository
{
    public interface ITagRepository
    {
        Task<List<TagModel>> GetTagsAsync(
            Guid blogPostId,
            CancellationToken token);

        Task<TagModel?> GetTagByIdAsync(
            Guid tagId,
            CancellationToken token);

        Task<TagModel> AddTagAsync(
            TagModel tag,
            CancellationToken token);

        Task<TagModel> UpdateTagAsync(
            TagModel tag,
            CancellationToken token);

        Task<bool> DeleteTagAsync(
            Guid tagId,
            CancellationToken token);
    }
}