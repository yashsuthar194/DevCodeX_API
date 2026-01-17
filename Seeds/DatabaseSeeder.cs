using Microsoft.Extensions.Logging;

namespace DevCodeX_API.Seeds
{
    /// <summary>
    /// Orchestrates the execution of all database seeders.
    /// Seeders are executed in order based on their Order property.
    /// </summary>
    public class DatabaseSeeder
    {
        private readonly IEnumerable<ISeeder> _seeders;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(IEnumerable<ISeeder> seeders, ILogger<DatabaseSeeder> logger)
        {
            _seeders = seeders;
            _logger = logger;
        }

        /// <summary>
        /// Runs all registered seeders in order.
        /// Each seeder checks if its table is empty before seeding.
        /// </summary>
        public async Task SeedAllAsync()
        {
            _logger.LogInformation("Starting database seeding...");

            // Order seeders by their Order property
            var orderedSeeders = _seeders.OrderBy(s => s.Order).ToList();

            if (!orderedSeeders.Any())
            {
                _logger.LogInformation("No seeders registered.");
                return;
            }

            foreach (var seeder in orderedSeeders)
            {
                try
                {
                    _logger.LogInformation("Running seeder: {SeederName}", seeder.GetType().Name);
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error running seeder: {SeederName}", seeder.GetType().Name);
                    throw; // Re-throw to prevent partial seeding
                }
            }

            _logger.LogInformation("Database seeding completed successfully.");
        }
    }
}
