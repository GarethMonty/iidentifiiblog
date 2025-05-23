namespace IIdentifii.Blog.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> 
        where TProgram : class
    {
        public Action<IServiceProvider>? SeedCallback { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing DbContextOptions registration
                ServiceDescriptor? descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Register InMemory test DB
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));

                ServiceProvider sp = services.BuildServiceProvider();

                using IServiceScope scope = sp.CreateScope();
                IServiceProvider scopedServices = scope.ServiceProvider;
                AppDbContext db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                SeedCallback?.Invoke(scopedServices);
            });
        }
    }
}
