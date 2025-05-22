namespace IIdentifii.Blog.Repository
{
    internal class CommentRepository : ICommentRepository
    {
        #region Fields

        private readonly AppDbContext _dbContext;

        private readonly DbSet<CommentModel> _set;

        #endregion

        #region Constructor Methods

        public CommentRepository(
            AppDbContext blogDbContext)
        {
            _dbContext = blogDbContext;

            _set = blogDbContext.Comments;
        }

        #endregion

        #region Methods

        public async Task<List<CommentModel>> GetCommentsAsync(
            CommentRequest request,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            IQueryable<CommentModel> query = _set
                .AsNoTracking()
                .AsQueryable();

            if (request.BlogPostId is not null)
            {
                query = query
                    .Where(x => x.BlogPostId == request.BlogPostId.Value);
            }
            if (request.UserId is not null)
            {
                query = query
                    .Where(x => x.UserId == request.UserId.Value);
            }

            if (request.DateFilter is not null)
            {
                query = query
                    .Where(x => x.CreatedAt >= request.DateFilter.From && x.CreatedAt <= request.DateFilter.To);
            }

            if (request.Filter is not null && !string.IsNullOrEmpty(request.Filter.Query))
            {
                query = query
                    .Where(x => x.Content.Contains(request.Filter.Query));
            }

            if (request.Paging is not null)
            {
                query = query
                    .Skip((request.Paging.Page - 1) * request.Paging.PageSize)
                    .Take(request.Paging.PageSize);
            }

            return await query.ToListAsync(token);
        }

        public async Task<CommentModel?> GetCommentByIdAsync(
            Guid commentId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == commentId, token);
        }

        public async Task<CommentModel> AddCommentAsync(
            CommentModel comment,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            await _set.AddAsync(comment, token);

            await _dbContext.SaveChangesAsync(token);

            return comment;
        }

        public async Task<CommentModel> UpdateCommentAsync(
            CommentModel comment,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            _set.Update(comment);

            await _dbContext.SaveChangesAsync(token);

            return comment;
        }

        public async Task<bool> DeleteCommentAsync(
            Guid commentId,
            CancellationToken token)
        {
            CommentModel? comment = await GetCommentByIdAsync(commentId, token);

            if (comment != null)
            {
                _set.Remove(comment);

                await _dbContext.SaveChangesAsync(token);
            }

            return comment != null;
        }

        #endregion
    }
}
