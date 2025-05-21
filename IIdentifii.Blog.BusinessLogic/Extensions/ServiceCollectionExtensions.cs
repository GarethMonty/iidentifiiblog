using IIdentifii.Blog.BusinessLogic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(
            this IServiceCollection services)
        {
            
            services
                .AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
