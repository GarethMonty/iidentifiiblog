namespace IIdentifii.Blog.Repository
{
    internal class TagRepository : ITagRepository
    {
        #region Fields

        private readonly AppDbContext _dbContext;

        private readonly DbSet<TagModel> _set;

        #endregion

        #region Constructor Methods

        public TagRepository(
            AppDbContext blogDbContext)
        {
            _dbContext = blogDbContext;

            _set = blogDbContext.Tags;
        }

        #endregion

        #region Methods

        public async Task<List<TagModel>> GetTagsAsync(
            Guid blogPostId,
            CancellationToken token)
        {
            return await _set
                .AsNoTracking()
                .Where(x => x.BlogPostId == blogPostId)
                .ToListAsync(token);
        }

        public async Task<TagModel?> GetTagByIdAsync(
            Guid tagId,
            CancellationToken token)
        {
            return await _set
                .FirstOrDefaultAsync(x => x.Id == tagId, token);
        }

        public async Task<TagModel> AddTagAsync(
            TagModel tag,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(tag, nameof(tag));

            await _set.AddAsync(tag, token);

            await _dbContext.SaveChangesAsync(token);

            return tag;
        }

        public async Task<TagModel> UpdateTagAsync(
            TagModel tag,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(tag, nameof(tag));

            _set.Update(tag);

            await _dbContext.SaveChangesAsync(token);

            return tag;
        }

        public async Task<bool> DeleteTagAsync(
            Guid tagId,
            CancellationToken token)
        {
            TagModel? tag = await GetTagByIdAsync(tagId, token);

            if (tag != null)
            {
                _set.Remove(tag);

                await _dbContext.SaveChangesAsync(token);
            }

            return (tag != null);
        }


        #endregion
    }
}
