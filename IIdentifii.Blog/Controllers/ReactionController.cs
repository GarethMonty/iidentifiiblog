namespace IIdentifii.Blog.Controllers
{
    /// <summary>
    /// Controller for managing likes on blog posts, including creating, removing, retrieving likes and reaction count.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/blog/post/{blogPostId:guid}/reaction/{reactionType}")]
    public class ReactionController : ControllerBase
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly IReactionService _likeService;

        #endregion

        #region Constructor Methods

        public ReactionController(
            IRequestContextService requestContextService,
            IReactionService likeService)
        {
            _requestContextService = requestContextService;

            _likeService = likeService;
        }

        #endregion

        /// <summary>
        /// Retrieves all likes for a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post.</param>
        /// <param name="reactionType">The enum type of a reaction (Like, Dislike, etc)</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A list of likes.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<List<Reaction>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReactionsAsync(
            [FromRoute] Guid blogPostId,
            [FromRoute] ReactionType reactionType,
            CancellationToken token)
        {
            ApiResponse<List<Reaction>> response = await _likeService.GetReactionsAsync(blogPostId, reactionType, token);

            return response.ToResult();
        }

        /// <summary>
        /// Retrieves the total number of likes for a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post.</param>
        /// <param name="reactionType">The enum type of a reaction (Like, Dislike, etc)</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The reaction count as an integer.</returns>
        [HttpGet("count")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReactionCountAsync(
            [FromRoute] Guid blogPostId,
            [FromRoute] ReactionType reactionType,
            CancellationToken token)
        {
            ApiResponse<int> response = await _likeService.GetReactionCountAsync(blogPostId, reactionType, token);

            return response.ToResult();
        }

        /// <summary>
        /// Creates a reaction for the current user on the specified blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to reaction.</param>
        /// <param name="reactionType">The enum type of a reaction (Like, Dislike, etc)</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The created reaction object.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Reaction>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Reaction>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateReactionAsync(
            [FromRoute] Guid blogPostId,
            [FromRoute] ReactionType reactionType,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Reaction>.Failure($"User not found").ToResult();
            }

            ApiResponse<Reaction> response = await _likeService.CreateReactionAsync(blogPostId, userId, reactionType, token);

            return response.ToResult();
        }

        /// <summary>
        /// Changes a reaction for the current user on the specified blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to reaction.</param>
        /// <param name="reactionType">The enum type of a reaction (Like, Dislike, etc)</param>
        /// <param name="previous">The enum type of the previous selected reaction (Like, Dislike, etc)</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The created reaction object.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpPatch]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Reaction>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Reaction>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeReactionAsync(
            [FromRoute] Guid blogPostId,
            [FromRoute] ReactionType reactionType,
            [FromQuery] ReactionType previous,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Reaction>.Failure($"User not found").ToResult();
            }

            ApiResponse<Reaction> response = await _likeService.CreateReactionAsync(blogPostId, userId, reactionType, token);

            return response.ToResult();
        }

        /// <summary>
        /// Removes the current user's reaction from the specified blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to unlike.</param>
        /// <param name="reactionType">The enum type of a reaction (Like, Dislike, etc)</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A boolean indicating whether the reaction was successfully removed.</returns>
        [Authorize(Roles = RoleConstants.User)]
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteReactionAsync(
            [FromRoute] Guid blogPostId,
            [FromRoute] ReactionType reactionType,
            CancellationToken token)
        {
            if (!_requestContextService.TryGetUserId(out Guid userId))
            {
                return ApiResponse<Reaction>.Failure($"User not found").ToResult();
            }

            ApiResponse<bool> response = await _likeService.DeleteReactionAsync(blogPostId, userId, reactionType, token);

            return response.ToResult();
        }

    }
}
