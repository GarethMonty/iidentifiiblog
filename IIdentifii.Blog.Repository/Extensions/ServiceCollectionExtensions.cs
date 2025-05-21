using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Sql");

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'Sql' is not configured.");
            }

            services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddAuthRepositoryServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDefaultIdentity<IdentityUser>(
                    options => 
                        options.SignIn.RequireConfirmedAccount = true
                )
                .AddUserStore<AuthDbContext>();

            return services;
        }
    }
}
