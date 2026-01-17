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
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<CodeXContext>();
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
//    try
//    {
//        // Step 1: Apply pending migrations
//        logger.LogInformation("Checking for pending database migrations...");
        
//        if (dbContext.Database.GetPendingMigrations().Any())
//        {
//            logger.LogInformation("Applying pending migrations...");
//            dbContext.Database.Migrate();
//            logger.LogInformation("Database migrations applied successfully.");
//        }
//        else
//        {
//            logger.LogInformation("Database is up to date. No migrations needed.");
//        }

//        // Step 2: Run database seeding (only seeds empty tables)
//        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
//        await seeder.SeedAllAsync();
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
//        throw; // Re-throw to prevent app from starting with a broken database
//    }
//}

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
