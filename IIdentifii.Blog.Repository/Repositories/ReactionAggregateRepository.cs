using Z.EntityFramework.Plus;

namespace IIdentifii.Blog.Repository
{
    internal class ReactionAggregateRepository : IReactionAggregateRepository
    {
        #region Fields

        private readonly AppDbContext _dbContext;

        private readonly DbSet<ReactionAggregateModel> _set;

        #endregion

        #region Constructor Methods

        public ReactionAggregateRepository(
            AppDbContext blogDbContext)
        {
            _dbContext = blogDbContext;

            _set = blogDbContext.ReactionAggregates;
        }

        #endregion

        #region Methods

        public async Task<List<ReactionAggregateModel>> GetReactionsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId)
                .ToListAsync(token);
        }

        public async Task<ReactionAggregateModel?> GetReactionAggregateByIdAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BlogPostId == blogPostId && x.Type == type, token);
        }

        public async Task<ReactionAggregateModel> AddReactionAggregateAsync(
            ReactionAggregateModel reactionAggregate,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(reactionAggregate, nameof(reactionAggregate));

            await _set.AddAsync(reactionAggregate, token);

            await _dbContext.SaveChangesAsync(token);

            return reactionAggregate;
        }

        public int? AddReactionUpdate(
            ReactionAggregateModel reactionAggregateModel)
        {
            ArgumentNullException.ThrowIfNull(reactionAggregateModel, nameof(reactionAggregateModel));

            _set.Update(reactionAggregateModel);

            return _dbContext.ChangeTracker.Entries().Count();
        }

        public async Task SaveReactionChangesAsync(
            CancellationToken token)
        {
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<bool> DeleteReactionAggregateAsync(
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            ReactionAggregateModel? reactionAggregate = await GetReactionAggregateByIdAsync(blogPostId, type, token);

            if (reactionAggregate != null)
            {
                _set.Remove(reactionAggregate);

                await _dbContext.SaveChangesAsync(token);
            }

            return reactionAggregate != null;
        }

        #endregion
    }
}
