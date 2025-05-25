namespace IIdentifii.Blog.BusinessLogic
{
    internal class BlogPostService : IBlogPostService
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly IBlogPostRepository _blogPostRepository;

        #endregion

        #region Constructor Methods

        public BlogPostService(
            IRequestContextService requestContextService,
            IBlogPostRepository blogPostRepository)
        {
            _requestContextService = requestContextService;

            _blogPostRepository = blogPostRepository;
        }

        #endregion

        #region Methods

        public async Task<PagedApiResponse<BlogPost>> GetBlogPostsAsync(
            BlogPostRequest request,
            CancellationToken token)
        {
            PagedResultModel<BlogPostModel> pagedModels = await _blogPostRepository.GetBlogPostsAsync(request, token);

            return PagedApiResponse<BlogPost>.Success(
                data: pagedModels.Items.Adapt<List<BlogPost>>(), 
                page: pagedModels.Page, 
                size: pagedModels.PageSize, 
                totalCount: pagedModels.TotalCount);
        }

        public async Task<ApiResponse<BlogPost>> GetBlogPostAsync(
            Guid id,
            CancellationToken token)
        {
            BlogPostModel? model = await _blogPostRepository.GetBlogPostAsync(id, token);

            if (model is null)
            {
                return ApiResponse<BlogPost>.NotFound($"Blog post with id {id} not found.");
            }

            return ApiResponse<BlogPost>.Success(model.Adapt<BlogPost>());
        }

        public async Task<ApiResponse<BlogPost>> CreateBlogPostAsync(
            CreateBlogPostRequest createRequest,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<BlogPost>.Failure($"User not found");
            }

            BlogPostModel model = createRequest.Adapt<BlogPostModel>();

            model.SetupForCreate(userId);

            model = await _blogPostRepository.CreateBlogPostAsync(model, token);

            return ApiResponse<BlogPost>.Success(model.Adapt<BlogPost>());
        }

        public async Task<ApiResponse<BlogPost>> UpdateBlogPostAsync(
            UpdateBlogPostRequest updateRequest,
            CancellationToken token)
        {
            BlogPostModel? model = await _blogPostRepository.GetBlogPostAsync(updateRequest.Id, token);

            if (model is null)
            {
                return ApiResponse<BlogPost>.NotFound($"Blog Post with id {updateRequest.Id} not found");
            }

            model.Title = updateRequest.Title;
            model.Content = updateRequest.Content;

            BlogPostModel updatedModel = await _blogPostRepository.UpdateBlogPostAsync(model, token);

            return ApiResponse<BlogPost>.Success(updatedModel.Adapt<BlogPost>());
        }

        public async Task<ApiResponse<bool>> DeleteBlogPostAsync(
            Guid id,
            CancellationToken token)
        {
            bool result = await _blogPostRepository.DeleteBlogPostAsync(id, token);

            if (result)
            {
                return ApiResponse<bool>.Success(result);
            }

            return ApiResponse<bool>.NotFound($"Blog post with id {id} not found.");
        }
        #endregion
    }
}
