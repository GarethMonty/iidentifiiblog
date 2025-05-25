namespace IIdentifii.Blog.Repository
{
    public class AppDbContext : IdentityDbContext<IIdentifiiUser, IdentityRole<Guid>, Guid>
    {
        #region Properties

        public DbSet<BlogPostModel> BlogPosts => Set<BlogPostModel>();

        public DbSet<CommentModel> Comments => Set<CommentModel>();

        public DbSet<ReactionModel> Reactions => Set<ReactionModel>();

        public DbSet<ReactionAggregateModel> ReactionAggregates => Set<ReactionAggregateModel>();

        public DbSet<TagModel> Tags => Set<TagModel>();

        #endregion

        #region Constructor Methods

        public AppDbContext(
            DbContextOptions<AppDbContext> options)
        : base(options)
        {}

        #endregion

        #region Methods

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new BlogPostConfig());
            builder.ApplyConfiguration(new ReactionConfig());
            builder.ApplyConfiguration(new ReactionAggregateConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new TagConfig());
        }

        #endregion
    }
}
