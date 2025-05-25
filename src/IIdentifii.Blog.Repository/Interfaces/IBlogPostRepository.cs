namespace IIdentifii.Blog.Repository
{
    public interface IBlogPostRepository
    {
        Task<PagedResultModel<BlogPostModel>> GetBlogPostsAsync(
            BlogPostRequest blogPostRequest,
            Guid userId,
            CancellationToken token);

        Task<BlogPostModel?> GetBlogPostAsync(
            Guid id,
            Guid userId,
            CancellationToken token);

        Task<BlogPostModel> CreateBlogPostAsync(
            BlogPostModel blogPostModel,
            CancellationToken token);

        Task<BlogPostModel> UpdateBlogPostAsync(
            BlogPostModel blogPostModel,
            CancellationToken token);

        Task<bool> DeleteBlogPostAsync(
            Guid id,
            CancellationToken token);
    }
}