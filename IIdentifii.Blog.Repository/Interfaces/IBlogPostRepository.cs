namespace IIdentifii.Blog.Repository
{
    public interface IBlogPostRepository
    {
        Task<List<BlogPostModel>> GetBlogPostsAsync(
            BlogPostRequest blogPostRequest,
            CancellationToken token);

        Task<List<BlogPostModel>> GetBlogPostsOfAuthorAsync(
            BlogPostRequest blogPostRequestd,
            CancellationToken token);

        Task<BlogPostModel?> GetBlogPostAsync(
            Guid id,
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