using DevCodeX_API.Context;
using DevCodeX_API.Helpers;
using DevCodeX_API.Middlewares;
using DevCodeX_API.Seeds;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// Configure Services
// ============================================
// Register all application services using extension methods
// This follows the Dependency Inversion Principle - high-level modules depend on abstractions
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// ============================================
// Auto-Apply Database Migrations & Seeding
// ============================================
// This will create the database if it doesn't exist, apply pending migrations, and seed data
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CodeXContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // MigrateAsync() will:
        // 1. Create the database if it doesn't exist
        // 2. Create __EFMigrationsHistory table if needed
        // 3. Apply all pending migrations
        logger.LogInformation("Applying database migrations (will create database if needed)...");
        
        try
        {
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Npgsql.PostgresException ex) when (ex.SqlState == "42P07") // 42P07 = relation already exists
        {
            // Tables already exist but migration wasn't tracked
            // This can happen if tables were created manually or migration history was cleared
            logger.LogWarning("Tables already exist (Error: {Message}). Marking migrations as applied...", ex.MessageText);
            
            // Get pending migrations and mark them as applied
            var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToList();
            foreach (var migration in pendingMigrations)
            {
                var sql = @"
                    INSERT INTO dbo.""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"") 
                    VALUES ({0}, {1}) 
                    ON CONFLICT DO NOTHING";
                await dbContext.Database.ExecuteSqlRawAsync(sql, migration, "8.0.4");
                logger.LogInformation("Marked migration as applied: {Migration}", migration);
            }
        }
        
        // Run database seeding (only seeds empty tables)
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAllAsync();
    }
    catch (Exception ex)
    {
        // Log error but don't crash the API - let it start anyway
        // This allows the API to be debugged and database issues to be fixed manually
        logger.LogError(ex, "An error occurred while migrating or seeding the database. API will continue starting.");
    }
}

// ============================================
// Configure Middleware Pipeline
// ============================================
// Order matters! Middlewares are executed in the order they are added

// 1. Exception Handling - Should be first to catch all exceptions
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// 2. HTTPS Redirection - Only in development (Render handles SSL at proxy level)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// 3. Request Logging Middleware - Log all incoming requests
app.UseRequestLogging();

// 4. CORS - Must be before authorization
app.UseCors(ServiceExtension.AllowCorsPolicy);

// 5. Swagger/OpenAPI Documentation
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DevCodeX API v1");
        options.RoutePrefix = "swagger"; // Access at /swagger
    });
}

// 6. Routing
app.UseRouting();

// 7. Authentication & Authorization (if needed in future)
// app.UseAuthentication();
// app.UseAuthorization();

// 8. Map Controllers
app.MapControllers();

// ============================================
// Error Handling Endpoint
// ============================================
app.MapGet("/error", () => Results.Problem("An error occurred processing your request."))
   .ExcludeFromDescription();

// ============================================
// Health Check Endpoint
// ============================================
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
})).ExcludeFromDescription();

// ============================================
// Run Application
// ============================================
app.Logger.LogInformation("DevCodeX API starting up...");
app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

app.Run();
