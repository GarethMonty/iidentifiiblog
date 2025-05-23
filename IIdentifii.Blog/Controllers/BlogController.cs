namespace IIdentifii.Blog.Controllers
{
    /// <summary>
    /// Controller for managing blog posts, including creation, retrieval, updates, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/blog/post")]
    public class BlogController : ControllerBase
    {
        #region Fields

        private readonly IBlogPostService _blogPostService;

        #endregion

        #region Constructor Methods

        public BlogController(
            IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        #endregion

        /// <summary>
        /// Retrieves a paged list of blog posts using optional filtering, sorting, and paging.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to query and page blog posts by author, date range, keywords.
        /// </remarks>
        /// <param name="request">The filtering, sorting, and paging request object.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A list of blog posts matching the specified criteria.</returns>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedApiResponse<BlogPost>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedApiResponse<BlogPost>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PagedApiResponse<BlogPost>), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(BlogPostRequest), typeof(BlogPostRequestExample))]
        public async Task<IActionResult> GetBlogPostsAsync(
            [FromBody] BlogPostRequest request,
            CancellationToken token)
        {
            PagedApiResponse<BlogPost> pagedResponse = await _blogPostService.GetBlogPostsAsync(request, token);

            return pagedResponse.ToResult();
        }

        /// <summary>
        /// Retrieves a single blog post by its unique ID.
        /// </summary>
        /// <param name="blogPostId">The unique identifier of the blog post.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The blog post with the specified ID.</returns>
        [HttpGet("{blogPostId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBlogPostAsync(
            [FromRoute] Guid blogPostId,
            CancellationToken token)
        {
            ApiResponse<BlogPost> response = await _blogPostService.GetBlogPostAsync(blogPostId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="request">The request payload containing blog post title and content.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The newly created blog post.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(CreateBlogPostRequest), typeof(CreateBlogPostRequestExample))]
        public async Task<IActionResult> CreateBlogPostAsync(
            [FromBody] CreateBlogPostRequest request,
            CancellationToken token)
        {
            ApiResponse<BlogPost> response = await _blogPostService.CreateBlogPostAsync(request, token);

            return response.ToResult();
        }

        /// <summary>
        /// Updates an existing blog post by ID.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to update.</param>
        /// <param name="request">The updated blog post data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The updated blog post.</returns>
        [FeatureGate(FeatureFlagConstants.EnablePostUpdate)]
        [Authorize(Roles = RoleConstants.User)]
        [HttpPatch("{blogPostId:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<BlogPost>), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(UpdateBlogPostRequest), typeof(UpdateBlogPostRequestExample))]
        public async Task<IActionResult> UpdateBlogPostAsync(
            [FromRoute] Guid blogPostId,
            [FromBody] UpdateBlogPostRequest request,
            CancellationToken token)
        {
            request.Id = blogPostId;
            ApiResponse<BlogPost> response = await _blogPostService.UpdateBlogPostAsync(request, token);

            return response.ToResult();
        }

        /// <summary>
        /// Deletes a blog post by ID.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to delete.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A boolean indicating whether the delete operation was successful.</returns>
        [FeatureGate(FeatureFlagConstants.EnablePostDelete)]
        [Authorize(Roles = RoleConstants.User)]
        [HttpDelete("{blogPostId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBlogPostAsync(
            [FromRoute] Guid blogPostId,
            CancellationToken token)
        {
            ApiResponse<bool> response = await _blogPostService.DeleteBlogPostAsync(blogPostId, token);

            return response.ToResult();
        }
    }
}
