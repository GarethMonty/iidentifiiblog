namespace IIdentifii.Blog.Repository
{
    internal class BlogPostRepository
    {
        #region Fields

        private readonly DbSet<BlogPostModel> _set;
        #endregion

        #region Constructor Methods

        public BlogPostRepository(
            BlogDbContext blogDbContext)
        {
            _set = blogDbContext.BlogPosts;
        }

        #endregion

        #region Methods

        public async Task<List<BlogPostModel>> GetBlogPostsAsync(
            BlogPostRequest blogPostRequest,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(blogPostRequest, nameof(blogPostRequest));

            IQueryable<BlogPostModel> query = _set
                .AsNoTracking()
                .AsQueryable();

            if (blogPostRequest.AuthorId is not null)
            {
                query = query
                    .Where(x => x.AuthorId == blogPostRequest.AuthorId.Value);
            }

            if(blogPostRequest.DateFilter is not null)
            {
                query = query
                    .Where(x => x.PublishedAt >= blogPostRequest.DateFilter.From && x.PublishedAt <= blogPostRequest.DateFilter.To);
            }

            if(blogPostRequest.Filter is not null)
            {
                query = query
                    .Where(x => x.Title.Contains(blogPostRequest.Filter.Query) || x.Content.Contains(blogPostRequest.Filter.Query));
            }

            if(blogPostRequest.Paging is not null)
            {
                // Apply paging logic here
            }

            return await query.ToListAsync(token);
        }

        #endregion
    }
}
