namespace IIdentifii.Blog.Repository
{
    public class BlogDbContext : DbContext
    {
        public DbSet<BlogPostModel> BlogPosts => Set<BlogPostModel>();

        public DbSet<CommentModel> Comments => Set<CommentModel>();

        public DbSet<LikeModel> Likes => Set<LikeModel>();

        public DbSet<ModeratorFlagModel> ModeratorFlags => Set<ModeratorFlagModel>();

        public BlogDbContext(
            DbContextOptions<BlogDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new BlogPostConfig());
            builder.ApplyConfiguration(new LikeConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new ModeratorFlagConfig());
            builder.ApplyConfiguration(new AuthorConfig());
        }
    }
}
