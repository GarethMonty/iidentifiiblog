namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCacheServices(
            this IServiceCollection services)
        {
            
            services
                .AddMemoryCache(options =>
                {
                    options.SizeLimit = 1024 * 1024 * 100; // 100 MB
                });

            return services;
        }
    }
}
