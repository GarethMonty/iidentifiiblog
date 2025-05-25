using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryServices(
            this IServiceCollection services,
            IHostEnvironment environment,
            IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Sql");

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'Sql' is not configured.");
            }

            if (!environment.IsEnvironment("Testing"))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(connectionString)
                        .AddInterceptors(new SoftDeleteInterceptor()));
                }
                else
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options
                            .UseInMemoryDatabase("LocalDB")
                            .AddInterceptors(new SoftDeleteInterceptor()));
                }
            }

            services
                .AddScoped<IBlogPostRepository, BlogPostRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<IReactionRepository, ReactionRepository>()
                .AddScoped<IReactionAggregateRepository, ReactionAggregateRepository>()
                .AddScoped<ITagRepository, TagRepository>();

            return services;
        }
    }
}
