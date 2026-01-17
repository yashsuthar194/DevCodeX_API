using DevCodeX_API.Context;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevCodeX_API.Seeds
{
    /// <summary>
    /// Seeds the Technology table with initial data.
    /// Only runs if the Technology table is empty.
    /// </summary>
    public class TechnologySeeder : ISeeder
    {
        private readonly CodeXContext _context;
        private readonly ILogger<TechnologySeeder> _logger;

        public TechnologySeeder(CodeXContext context, ILogger<TechnologySeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int Order => 1; // Technologies should be seeded first (no dependencies)

        public async Task SeedAsync()
        {
            // Check if the table is empty
            if (await _context.Technology.AnyAsync())
            {
                _logger.LogInformation("Technology table already has data. Skipping seed.");
                return;
            }

            _logger.LogInformation("Seeding Technology table...");

            var technologies = new List<Technology>
            {
                // Languages
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "C#",
                    Description = "A modern, object-oriented programming language developed by Microsoft.",
                    TechnologyType = TechnologyType.Language,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "JavaScript",
                    Description = "A versatile scripting language for web development.",
                    TechnologyType = TechnologyType.Language,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "TypeScript",
                    Description = "A typed superset of JavaScript that compiles to plain JavaScript.",
                    TechnologyType = TechnologyType.Language,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "Python",
                    Description = "A high-level, interpreted programming language known for its readability.",
                    TechnologyType = TechnologyType.Language,
                    CreatedAt = DateTime.UtcNow
                },

                // Frameworks
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "ASP.NET Core",
                    Description = "A cross-platform, high-performance framework for building modern web applications.",
                    TechnologyType = TechnologyType.Framework,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "Angular",
                    Description = "A platform for building mobile and desktop web applications.",
                    TechnologyType = TechnologyType.Framework,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "React",
                    Description = "A JavaScript library for building user interfaces.",
                    TechnologyType = TechnologyType.Framework,
                    CreatedAt = DateTime.UtcNow
                },

                // Libraries
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "Entity Framework Core",
                    Description = "A modern object-database mapper for .NET.",
                    TechnologyType = TechnologyType.Library,
                    CreatedAt = DateTime.UtcNow
                },

                // Databases
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "PostgreSQL",
                    Description = "A powerful, open source object-relational database system.",
                    TechnologyType = TechnologyType.Database,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "SQL Server",
                    Description = "A relational database management system developed by Microsoft.",
                    TechnologyType = TechnologyType.Database,
                    CreatedAt = DateTime.UtcNow
                },

                // Tools
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "Docker",
                    Description = "A platform for developing, shipping, and running applications in containers.",
                    TechnologyType = TechnologyType.Tool,
                    CreatedAt = DateTime.UtcNow
                },
                new Technology
                {
                    Id = Guid.NewGuid(),
                    Name = "Git",
                    Description = "A distributed version control system for tracking changes in source code.",
                    TechnologyType = TechnologyType.Tool,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await _context.Technology.AddRangeAsync(technologies);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Technology table seeded with {Count} records.", technologies.Count);
        }
    }
}
