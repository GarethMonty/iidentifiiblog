using IIdentifii.Blog.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            AuthSettings? settings = configuration.GetSection(AuthSettings.Key).Get<AuthSettings>();

            if (settings == null)
            {
                throw new ArgumentNullException($"Configuration section '{AuthSettings.Key}' is not configured.");
            }

            settings.Validate();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = settings.AllowedUserNameCharacters;
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = settings.LoginPath;
                options.AccessDeniedPath = settings.AccessDeniedPath;
                options.SlidingExpiration = true;
            });

            services
                .AddIdentity<IIdentifiiUser, IdentityRole<Guid>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                //JWT Bearer Auth
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    byte[] key = Encoding.UTF8.GetBytes(settings.Secret);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            return services;
        }

        public static IServiceCollection AddContextServices(
            this IServiceCollection services)
        {
            services
                .AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddDocumentationServices(
            this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                                  "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                                  "Example: 'Bearer abc123token'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                string xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlPath);

                string sharedXmlPath = Path.Combine(AppContext.BaseDirectory, "IIdentifii.Blog.Shared.xml");
                c.IncludeXmlComments(sharedXmlPath);
            });

            services.AddOpenApi();

            return services;
        }

        public static IServiceCollection AddFeatureFlagServices(
            this IServiceCollection services)
        {
            services
                .AddFeatureManagement()
                .WithTargeting()
                .UseDisabledFeaturesHandler(new ApiFeatureNotAvailableHandler());

            return services;
        }

        public static IServiceCollection AddBackgroundProcessingServices(
            this IServiceCollection services)
        {
            services
                .AddHostedService<ReactionProcessingBackgroundService>();

            return services;
        }

    }
}
