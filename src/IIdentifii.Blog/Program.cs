namespace IIdentifii.Blog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddAuthServices(builder.Configuration)
                .AddCacheServices()
                .AddContextServices()
                .AddBusinessLogicServices()
                .AddRepositoryServices(builder.Environment, builder.Configuration)
                .AddBackgroundProcessingServices()
                .AddDocumentationServices()
                .AddSettingServices(builder.Configuration)
                .AddFeatureFlagServices()
                .AddMappingServices();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services
                .AddControllers(options =>
                {
                    options.Filters.Add<ValidateModelAttribute>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddRazorPages();

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapSwagger();

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.SpecUrl("/swagger/v1/swagger.json");
                c.DocumentTitle = "IIdentifii Blog API Docs";
            });

            await app.RunMigrationsAsync();

            await app.SeedExampleDataAsync();

            await app.RunAsync();
        }
    }
}
