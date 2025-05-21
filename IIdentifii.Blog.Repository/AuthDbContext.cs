namespace IIdentifii.Blog.Repository
{
    public class AuthDbContext : IdentityDbContext<IIdentifiiUser, IdentityRole<Guid>, Guid>
    {
        public AuthDbContext(
            DbContextOptions<AuthDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}