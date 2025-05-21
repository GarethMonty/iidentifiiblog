namespace IIdentifii.Blog.Repository
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(
            DbContextOptions<AuthDbContext> options) 
        : base(options)
        {
        }
    }
}
