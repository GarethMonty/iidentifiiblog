namespace IIdentifii.Blog.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly IRequestContextService _requestContextService;

        private readonly IBlogPostService _blogPostService;

        #endregion

        public HomeController(
            IRequestContextService requestContextService,
            IBlogPostService blogPostService)
        {
            _requestContextService = requestContextService;

            _blogPostService = blogPostService ?? throw new ArgumentNullException(nameof(blogPostService));
        }

        public async Task<IActionResult> Index(
            BlogPostRequest blogPostRequest,
            CancellationToken token)
        {
            _requestContextService.TryGetUserId(out Guid userId);

            blogPostRequest ??= new BlogPostRequest();

            PagedApiResponse<BlogPost> posts = await _blogPostService.GetBlogPostsAsync(blogPostRequest, userId, token);

            return View(posts);
        }
    }
}
