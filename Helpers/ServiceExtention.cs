using DevCodeX_API.Context;
using DevCodeX_API.Repositories.Implementation;
using DevCodeX_API.Repositories.Interfaces;
using DevCodeX_API.Seeds;
using DevCodeX_API.Services.Implementation;
using DevCodeX_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DevCodeX_API.Helpers
{
    /// <summary>
    /// Service registration extensions following SOLID principles.
    /// Each method has a single responsibility and is open for extension.
    /// </summary>
    public static class ServiceExtension
    {
        public const string AllowCorsPolicy = "AllowCors";

        /// <summary>
        /// Main extension method to register all application services.
        /// Orchestrates other extension methods for better organization.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseContext(configuration);
            services.AddRepositories();
            services.AddBusinessServices();
            services.AddSeeders();
            services.AddCorsPolicy(configuration);
            services.AddApiDocumentation();
            services.AddControllersWithJsonOptions();

            return services;
        }

        /// <summary>
        /// Configures Entity Framework Core with PostgreSQL.
        /// Dependency Inversion Principle: Depends on IConfiguration abstraction.
        /// </summary>
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection string is not configured.");

            services.AddDbContext<CodeXContext>(options =>
            {
                options.UseNpgsql(connectionString);
                
                // Enable sensitive data logging in development
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            return services;
        }

        /// <summary>
        /// Registers all repository implementations.
        /// Single Responsibility: Only handles repository registration.
        /// </summary>
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register generic base repository for all entities
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseReposioty<>));

            // Specific repository registrations can be added here if needed
            // Example: services.AddScoped<ISpecificRepository, SpecificRepository>();

            return services;
        }

        /// <summary>
        /// Registers all business service implementations.
        /// Open/Closed Principle: Easy to add new services without modifying existing code.
        /// </summary>
        private static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register all service implementations
            services.AddScoped<ITechnologyService, TechnologyService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IAssetService, AssetService>();

            return services;
        }

        /// <summary>
        /// Registers all database seeders.
        /// Add new seeders here as needed.
        /// </summary>
        private static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            // Register individual seeders
            services.AddScoped<ISeeder, TechnologySeeder>();
            // Add more seeders here as needed:
            // services.AddScoped<ISeeder, QuestionSeeder>();

            // Register the seeder orchestrator
            services.AddScoped<DatabaseSeeder>();

            return services;
        }

        /// <summary>
        /// Configures CORS policy for cross-origin requests.
        /// </summary>
        private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowCorsPolicy, builder =>
                {
                    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                        ?? new[] { "*" };

                    if (allowedOrigins.Contains("*"))
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }
                    else
                    {
                        builder.WithOrigins(allowedOrigins)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Configures Swagger/OpenAPI documentation.
        /// </summary>
        private static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DevCodeX API",
                    Version = "v1",
                    Description = "DevCodeX API for managing technologies, questions, answers, and assets",
                    Contact = new OpenApiContact
                    {
                        Name = "DevCodeX Team"
                    }
                });

                // Add XML comments if available
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        /// <summary>
        /// Configures controllers with custom JSON serialization options.
        /// Preserves property names (PascalCase) instead of converting to camelCase.
        /// </summary>
        private static IServiceCollection AddControllersWithJsonOptions(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Preserve property naming (don't convert to camelCase)
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    
                    // Handle circular references
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    
                    // Write indented JSON for better readability in development
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            return services;
        }
    }
}
