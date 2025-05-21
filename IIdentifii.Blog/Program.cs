using IIdentifii.Blog.Repository;

namespace IIdentifii.Blog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddAuthServices(builder.Configuration)
                .AddBusinessLogicServices()
                .AddCacheServices()
                .AddAuthRepositoryServices(builder.Configuration)
                .AddRepositoryServices(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();

            app.Run();

            await SeedHelpers.SeedRolesAndUsersAsync(app.Services);
        }
    }
}
