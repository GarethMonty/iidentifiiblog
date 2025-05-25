namespace IIdentifii.Blog.BusinessLogic
{
    public interface IBlogPostService
    {
        Task<PagedApiResponse<BlogPost>> GetBlogPostsAsync(
            BlogPostRequest request,
            Guid userId,
            CancellationToken token);

        Task<ApiResponse<BlogPost>> GetBlogPostAsync(
            Guid id,
            Guid userId,
            CancellationToken token);

        Task<ApiResponse<BlogPost>> CreateBlogPostAsync(
            CreateBlogPostRequest createRequest,
            CancellationToken token);

        Task<ApiResponse<BlogPost>> UpdateBlogPostAsync(
            UpdateBlogPostRequest updateRequest,
            CancellationToken token);

        Task<ApiResponse<bool>> DeleteBlogPostAsync(
            Guid id,
            CancellationToken token);
    }
}