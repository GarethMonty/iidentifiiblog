namespace IIdentifii.Blog.Controllers
{
    /// <summary>
    /// Handles operations related to blog post tags.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/blog/post/{blogPostId:guid}/tag")]
    public class TagController : ControllerBase
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly ITagService _tagService;

        #endregion

        #region Constructor Methods

        public TagController(
            IRequestContextService requestContextService,
            ITagService tagService)
        {
            _requestContextService = requestContextService;
            _tagService = tagService;
        }

        #endregion

        /// <summary>
        /// Retrieves all tags associated with a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A list of tags.</returns>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<List<Tag>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<Tag>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTagsAsync(
            [FromRoute] Guid blogPostId,
            CancellationToken token)
        {
            ApiResponse<List<Tag>> response = await _tagService.GetTagsAsync(blogPostId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Retrieves a specific tag by its ID.
        /// </summary>
        /// <param name="tagId">The ID of the tag.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A tag.</returns>
        [HttpGet("{tagId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTagAsync(
            [FromRoute] Guid tagId,
            CancellationToken token)
        {
            ApiResponse<Tag> response = await _tagService.GetTagAsync(tagId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Creates a new tag for a specific blog post. Requires Moderator role.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post.</param>
        /// <param name="request">The create tag request.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>The new tag.</returns>
        [Authorize(Roles = RoleConstants.Moderator)]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status401Unauthorized)]
        [SwaggerRequestExample(typeof(CreateTagRequest), typeof(CreateTagRequestExample))]
        public async Task<IActionResult> CreateTagAsync(
            [FromRoute] Guid blogPostId,
            [FromBody] CreateTagRequest request,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Tag>.Failure($"User not found").ToResult();
            }

            request.BlogPostId = blogPostId;

            ApiResponse<Tag> response = await _tagService.CreateTagAsync(request, userId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Updates an existing tag. Requires Moderator role.
        /// </summary>
        /// <param name="request">The update tag request.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>The updated tag.</returns>
        [Authorize(Roles = RoleConstants.Moderator)]
        [HttpPatch]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Tag>), StatusCodes.Status401Unauthorized)]
        [SwaggerRequestExample(typeof(UpdateTagRequest), typeof(UpdateTagRequestExample))]
        public async Task<IActionResult> UpdateTagAsync(
            [FromBody] UpdateTagRequest request,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Tag>.Failure($"User not found").ToResult();
            }

            ApiResponse<Tag> response = await _tagService.UpdateTagAsync(request, userId, token);

            return response.ToResult();
        }

        /// <summary>
        /// Deletes a tag by its ID. Requires Moderator role.
        /// </summary>
        /// <param name="tagId">The ID of the tag to delete.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>If was successfully deleted.</returns>
        [Authorize(Roles = RoleConstants.Moderator)]
        [HttpDelete("{tagId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTagAsync(
            [FromRoute] Guid tagId,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Tag>.Failure($"User not found").ToResult();
            }

            ApiResponse<bool> response = await _tagService.DeleteTagAsync(tagId, userId, token);

            return response.ToResult();
        }
    }
}
