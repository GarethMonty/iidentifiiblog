namespace IIdentifii.Blog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/blog/post/{blogPostId:guid}/tag")]
    public class TagController : ControllerBase
    {
        #region Fields

        private readonly ITagService _tagService;

        #endregion

        #region Constructor Methods

        public TagController(
            ITagService tagService)
        {
            _tagService = tagService;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetTagsAsync(
            [FromRoute] Guid blogPostId,
            CancellationToken token)
        {
            ApiResponse<List<Tag>> response = await _tagService.GetTagsAsync(blogPostId, token);

            return response.ToResult();
        }
    }
}
