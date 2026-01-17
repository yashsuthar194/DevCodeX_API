namespace DevCodeX_API.Seeds
{
    /// <summary>
    /// Interface for database seeders.
    /// Implement this interface to create seeders for specific entities.
    /// </summary>
    public interface ISeeder
    {
        /// <summary>
        /// Order in which this seeder should run (lower runs first).
        /// Use this to handle dependencies between seeders.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Seeds the data if the table is empty.
        /// </summary>
        /// <returns>Task indicating completion</returns>
        Task SeedAsync();
    }
}
