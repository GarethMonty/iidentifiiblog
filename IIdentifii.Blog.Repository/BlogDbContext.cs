namespace IIdentifii.Blog.Repository
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(
            DbContextOptions<BlogDbContext> options) 
        : base(options)
        {
        }
    }
}
