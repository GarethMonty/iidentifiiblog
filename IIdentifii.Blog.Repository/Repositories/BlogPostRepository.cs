using Z.EntityFramework.Plus;

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

        public async Task<List<BlogPostModel>> GetBlogPostsAsync(
            BlogPostRequest blogPostRequest,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(blogPostRequest, nameof(blogPostRequest));

            IQueryable<BlogPostModel> query = _set
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Comments)
                .Include(x => x.Tags)
                .Include(x => x.ReactionAggregates)
                .AsQueryable();

            if (blogPostRequest.AuthorId is not null)
            {
                query = query
                    .Where(x => x.AuthorId == blogPostRequest.AuthorId.Value);
            }

            if (blogPostRequest.DateFilter is not null)
            {
                query = query
                    .Where(x => x.PostedAt >= blogPostRequest.DateFilter.From && x.PostedAt <= blogPostRequest.DateFilter.To);
            }

            if (blogPostRequest.Filter is not null && !string.IsNullOrEmpty(blogPostRequest.Filter.Query))
            {
                query = query
                    .Where(x => x.Title.Contains(blogPostRequest.Filter.Query) || x.Content.Contains(blogPostRequest.Filter.Query));
            }

            if (blogPostRequest.ReactionFilter is not null)
            {
                if (blogPostRequest.ReactionFilter.IncludeEntities)
                {
                    query = query
                        .Include(x => x.Reactions);
                }

                if(blogPostRequest.ReactionFilter.ExcludedTypes.Count > 0)
                {
                    query = query
                        .IncludeFilter(x => x.Reactions.Where(r => !blogPostRequest.ReactionFilter.ExcludedTypes.Contains(r.Type)));
                }
            }

            if (blogPostRequest.Sort is not null)
            {
                if (blogPostRequest.Sort.SortOrder == SortOrderType.Descending)
                {
                    switch (blogPostRequest.Sort.SortBy)
                    {
                        default:
                        case SortByType.PostedAt:
                            query = query
                                .OrderByDescending(x => x.PostedAt);
                            break;
                        case SortByType.Title:
                            query = query
                                .OrderByDescending(x => x.Title);
                            break;
                        case SortByType.ReactionCount:
                            break;
                        case SortByType.Tags:
                            break;
                    }
                }
                else
                {
                    switch (blogPostRequest.Sort.SortBy)
                    {
                        default:
                        case SortByType.PostedAt:
                            query = query
                                .OrderBy(x => x.PostedAt);
                            break;
                        case SortByType.Title:
                            query = query
                                .OrderBy(x => x.Title);
                            break;
                        case SortByType.ReactionCount:
                            break;
                        case SortByType.Tags:
                            break;
                    }
                }
            }

            if (blogPostRequest.Paging is not null)
            {
                query = query
                    .Skip((blogPostRequest.Paging.Page - 1) * blogPostRequest.Paging.PageSize)
                    .Take(blogPostRequest.Paging.PageSize);
            }

            return await query.ToListAsync(token);
        }

        public Task<List<BlogPostModel>> GetBlogPostsOfAuthorAsync(
            BlogPostRequest blogPostRequest,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(blogPostRequest, nameof(blogPostRequest));
            ArgumentNullException.ThrowIfNull(blogPostRequest.AuthorId, nameof(blogPostRequest.AuthorId));

            return GetBlogPostsAsync(blogPostRequest, token);
        }

        public async Task<BlogPostModel?> GetBlogPostAsync(
            Guid id,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
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
