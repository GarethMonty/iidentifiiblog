namespace IIdentifii.Blog.Repository
{
    internal class BlogPostRepository : IBlogPostRepository
    {
        #region Fields

        private readonly AppDbContext _dbContext;

        private readonly DbSet<BlogPostModel> _set;

        #endregion

        #region Constructor Methods

        public BlogPostRepository(
            AppDbContext blogDbContext)
        {
            _dbContext = blogDbContext;

            _set = blogDbContext.BlogPosts;
        }

        #endregion

        #region Methods

        public async Task<PagedResultModel<BlogPostModel>> GetBlogPostsAsync(
            BlogPostRequest blogPostRequest,
            CancellationToken token)
        {
            blogPostRequest.Validate();

            ArgumentNullException.ThrowIfNull(blogPostRequest, nameof(blogPostRequest));

            IQueryable<BlogPostModel> baseQuery = _set
                .BeginFilter(blogPostRequest)
                .ApplyAuthorFilter()
                .ApplyDateFilter()
                .ApplyTextQueryFilter()
                .ApplyReactionFilter()
                .ApplyTagFilter()
                .BuildFilter();

            int totalCount = await baseQuery.CountAsync(token);

            IQueryable<BlogPostModel> pagedQuery = (baseQuery, blogPostRequest)
                .ApplySort()
                .ApplyPaging()
                .BuildFilter();

            List<BlogPostModel> items = await pagedQuery.ToListAsync(token);

            PagedResultModel<BlogPostModel> pagedResultModel = PagedResultModel<BlogPostModel>.CreateFromRequest(blogPostRequest.Paging);

            pagedResultModel.Items = items;
            pagedResultModel.TotalCount = totalCount;

            return pagedResultModel;
        }

        public async Task<BlogPostModel?> GetBlogPostAsync(
            Guid id,
            CancellationToken token)
        {
            return await _set
                .Include(x => x.Author)
                .Include(x => x.Comments)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<BlogPostModel> CreateBlogPostAsync(
            BlogPostModel blogPostModel,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(blogPostModel, nameof(blogPostModel));

            await _set.AddAsync(blogPostModel, token);

            await _dbContext.SaveChangesAsync(token);

            return blogPostModel;
        }

        public async Task<BlogPostModel> UpdateBlogPostAsync(
            BlogPostModel blogPostModel,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(blogPostModel, nameof(blogPostModel));

            _set.Update(blogPostModel);

            await _dbContext.SaveChangesAsync(token);

            return blogPostModel;
        }

        public async Task<bool> DeleteBlogPostAsync(
            Guid id,
            CancellationToken token)
        {
            BlogPostModel? blogPostModel = await GetBlogPostAsync(id, token);

            if (blogPostModel is null)
            {
                return false;
            }

            _set.Remove(blogPostModel);

            await _dbContext.SaveChangesAsync(token);

            return true;
        }
        #endregion
    }
}
