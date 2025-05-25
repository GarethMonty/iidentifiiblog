using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebApplicationExtensions
    {
        public static async Task RunMigrationsAsync(
            this WebApplication app, 
            CancellationToken token = default)
        {
            if (app.Environment.IsEnvironment("Testing"))
            {
                return;
            }

            await ApplyMigrationsAsync<AppDbContext>(app, token);
        }

        public static async Task SeedExampleDataAsync(
            this WebApplication app,
            CancellationToken token = default)
        {
            using IServiceScope scope = app.Services.CreateScope();

            if (app.Environment.IsEnvironment("Testing") || app.Environment.IsEnvironment("Development"))
            {
                AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await db.Database.EnsureDeletedAsync(token);
                await db.Database.EnsureCreatedAsync(token);
            }

            await SeedHelpers.GenerateTestDataEnvironment_Normal(scope.ServiceProvider);  
        }

        private static async Task ApplyMigrationsAsync<T>(
            this WebApplication app, 
            CancellationToken token = default)
            where T : DbContext
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;

                T context = provider.GetRequiredService<T>();

                await context.Database.EnsureCreatedAsync(token);

                await context.Database.MigrateAsync(token);
            }
        }
    }
}
