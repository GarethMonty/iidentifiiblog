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

        public async Task<PagedResultModel<CommentModel>> GetCommentsAsync(
            CommentRequest request,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            request.Validate();

            IQueryable<CommentModel> baseQuery = _set
                .BeginFilter(request)
                .ApplyBlogPostFilter()
                .ApplyUserFilter()
                .ApplyDateFilter()
                .ApplyTextQueryFilter()
                .BuildFilter();

            int totalCount = await baseQuery.CountAsync(token);

            IQueryable<CommentModel> pagedQuery = (baseQuery, request)
                .ApplyPaging()
                .BuildFilter();

            List<CommentModel> items = await pagedQuery.ToListAsync(token);

            PagedResultModel<CommentModel> pagedResultModel = PagedResultModel<CommentModel>.CreateFromRequest(request.Paging);

            pagedResultModel.Items = items;
            pagedResultModel.TotalCount = totalCount;

            return pagedResultModel;
        }

        public async Task<CommentModel?> GetCommentByIdAsync(
            Guid commentId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Include(c=> c.User)
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
