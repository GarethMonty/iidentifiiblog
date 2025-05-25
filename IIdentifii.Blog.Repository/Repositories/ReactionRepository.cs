namespace IIdentifii.Blog.Repository
{
    internal class ReactionRepository : IReactionRepository
    {
        #region Fields

        private readonly AppDbContext _dbContext;

        private readonly DbSet<ReactionModel> _set;

        #endregion

        #region Constructor Methods

        public ReactionRepository(
            AppDbContext blogDbContext)
        {
            _dbContext = blogDbContext;

            _set = blogDbContext.Reactions;
        }

        #endregion

        #region Methods

        public async Task<List<ReactionModel>> GetReactionsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId)
                .ToListAsync(token);
        }

        public async Task<List<ReactionModel>> GetReactionsByTypeAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId && x.Type == type)
                .ToListAsync(token);
        }

        public async Task<int> GetReactionCountAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId && x.Type == type)
                .CountAsync(token);
        }

        public async Task<ReactionModel?> GetReactionByIdAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BlogPostId == blogPostId && x.UserId == userId && x.Type == type, token);
        }

        public async Task<ReactionModel> AddReactionAsync(
            ReactionModel reaction,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(reaction, nameof(reaction));

            await _set.AddAsync(reaction, token);

            await _dbContext.SaveChangesAsync(token);

            return reaction;
        }

        public async Task<ReactionModel> UpdateReactionAsync(
            ReactionModel reaction,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(reaction, nameof(reaction));

            _set.Update(reaction);

            await _dbContext.SaveChangesAsync(token);

            return reaction;
        }

        public async Task<bool> DeleteReactionAsync(
            Guid blogPostId,
            Guid userId,
            ReactionType type,
            CancellationToken token)
        {
            ReactionModel? reaction = await GetReactionByIdAsync(blogPostId, userId, type, token);

            if (reaction != null)
            {
                _set.Remove(reaction);

                await _dbContext.SaveChangesAsync(token);
            }

            return reaction != null;
        }

        #endregion
    }
}
