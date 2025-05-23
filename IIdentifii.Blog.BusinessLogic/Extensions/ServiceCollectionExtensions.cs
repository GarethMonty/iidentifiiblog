namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(
            this IServiceCollection services)
        {
            
            services
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IRequestContextService, RequestContextService>()
                .AddScoped<IBlogPostService, BlogPostService>()
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<IReactionService, ReactionService>()
                .AddScoped<ITagService, TagService>();

            services
                .AddSingleton<IReactionHandler, ReactionHandler>();

            return services;
        }
    }
}
