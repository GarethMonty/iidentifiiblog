using Microsoft.AspNetCore.Mvc;

namespace IIdentifii.Blog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        #region Fields

        private readonly ILogger<BlogController> _logger;

        #endregion

        #region Constructor Methods

        public BlogController(
            ILogger<BlogController> logger)
        {
            _logger = logger;
        }

        #endregion

        [HttpGet]
        public IActionResult GetBlogPostsAsync(
            CancellationToken token)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetBlogPostAsync()
        {
            return Ok();
        }


    }
}
