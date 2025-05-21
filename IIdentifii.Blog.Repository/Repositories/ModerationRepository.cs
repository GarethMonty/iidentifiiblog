namespace IIdentifii.Blog.Repository
{
    internal class ModerationRepository : IModerationRepository
    {
        #region Fields

        private readonly DbSet<ModeratorFlagModel> _set;

        #endregion

        #region Constructor Methods

        public ModerationRepository(
            BlogDbContext blogDbContext)
        {
            _set = blogDbContext.Moderations;
        }

        #endregion

        #region Methods

        public async Task<List<ModeratorFlagModel>> GetModerationsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId)
                .ToListAsync(token);
        }

        public async Task<ModeratorFlagModel?> GetModerationByIdAsync(
            Guid moderationId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == moderationId, token);
        }

        public async Task<ModeratorFlagModel> AddModerationAsync(
            ModeratorFlagModel moderation,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(moderation, nameof(moderation));

            await _set.AddAsync(moderation, token);

            return moderation;
        }

        public async Task<ModeratorFlagModel> UpdateModerationAsync(
            ModeratorFlagModel moderation,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(moderation, nameof(moderation));

            _set.Update(moderation);

            return moderation;
        }

        public async Task<bool> DeleteModerationAsync(
            Guid moderationId,
            CancellationToken token)
        {
            ModeratorFlagModel? moderation = await GetModerationByIdAsync(moderationId, token);

            if (moderation != null)
            {
                _set.Remove(moderation);
            }

            return (moderation != null);
        }


        #endregion
    }
}
