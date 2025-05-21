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
            string? connectionString = configuration.GetConnectionString("Sql");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'Sql' is not configured.");
            }

            services
                .AddDbContext<AuthDbContext>(
                    options => options.UseSqlServer(connectionString)
                );

            services
                .AddDefaultIdentity<IIdentifiiUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
