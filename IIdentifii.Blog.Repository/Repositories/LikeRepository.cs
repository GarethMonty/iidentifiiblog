namespace IIdentifii.Blog.Repository
{
    internal class LikeRepository : ILikeRepository
    {
        #region Fields

        private readonly DbSet<LikeModel> _set;

        #endregion

        #region Constructor Methods

        public LikeRepository(
            BlogDbContext blogDbContext)
        {
            _set = blogDbContext.Likes;
        }

        #endregion

        #region Methods

        public async Task<List<LikeModel>> GetLikesAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId)
                .ToListAsync(token);
        }

        public async Task<int> GetLikeCountAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId)
                .CountAsync(token);
        }

        public async Task<LikeModel?> GetLikeByIdAsync(
            Guid blogPostId,
            Guid userId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BlogPostId == blogPostId && x.UserId == userId, token);
        }

        public async Task<LikeModel> AddLikeAsync(
            LikeModel like,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(like, nameof(like));

            await _set.AddAsync(like, token);

            return like;
        }

        public async Task<bool> DeleteLikeAsync(
            Guid blogPostId,
            Guid userId,
            CancellationToken token)
        {
            LikeModel? like = await GetLikeByIdAsync(blogPostId, userId, token);

            if (like != null)
            {
                _set.Remove(like);
            }

            return like != null;
        }

        #endregion
    }
}
