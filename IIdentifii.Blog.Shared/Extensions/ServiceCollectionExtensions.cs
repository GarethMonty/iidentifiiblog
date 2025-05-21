using IIdentifii.Blog.Shared;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddOptions();

            services
                .Configure<AuthSettings>(configuration.GetSection(AuthSettings.Key));

            return services;
        }
    }
}
