namespace IIdentifii.Blog.Controllers
{
    /// <summary>
    /// Controller for managing comments on blog posts, including create, read, update, and delete operations.
    /// </summary>
    [Authorize]
    [Route("api/blog/post/{blogPostId:guid}/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly ICommentService _commentService;

        #endregion

        #region Constructor Methods

        public CommentController(
            IRequestContextService requestContextService,
            ICommentService commentService)
        {
            _requestContextService = requestContextService;

            _commentService = commentService;
        }

        #endregion

        /// <summary>
        /// Retrieves a list of comments for a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post whose comments are being requested.</param>
        /// <param name="request">Filtering and paging parameters for the comment list.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A list of matching comments.</returns>
        [HttpGet]

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<List<Comment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<Comment>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCommentsAsync(
            [FromRoute] Guid blogPostId,
            [FromBody] CommentRequest request,
            CancellationToken token)
        {
            request.BlogPostId = blogPostId;

            ApiResponse<List<Comment>> response = await _commentService.GetCommentsAsync(request, token);

            return response.ToResult();
        }

        /// <summary>
        /// Retrieves a single comment by its unique ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to retrieve.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The comment if found.</returns>
        [HttpGet("{commentId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCommentAsync(
            [FromRoute] Guid commentId,
            CancellationToken token)
        {
            ApiResponse<Comment> response = await _commentService.GetCommentAsync(commentId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Creates a new comment on a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to comment on.</param>
        /// <param name="request">The comment creation data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The newly created comment.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCommentAsync(
            [FromRoute] Guid blogPostId,
            [FromBody] CreateCommentRequest request,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Comment>.Failure($"User not found").ToResult();
            }

            request.BlogPostId = blogPostId;

            ApiResponse<Comment> response = await _commentService.CreateCommentAsync(request, userId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="request">The updated comment data, including the ID.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The updated comment.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpPatch]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Comment>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCommentAsync(
            [FromBody] UpdateCommentRequest request,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Comment>.Failure($"User not found").ToResult();
            }

            ApiResponse<Comment> response = await _commentService.UpdateCommentAsync(request, userId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Deletes a specific comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A boolean indicating if the deletion was successful.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpDelete("{commentId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCommentAsync(
            [FromRoute] Guid commentId,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Comment>.Failure($"User not found").ToResult();
            }

            ApiResponse<bool> response = await _commentService.DeleteCommentAsync(commentId, userId, token);

            return response.ToResult();
        }
    }
}
