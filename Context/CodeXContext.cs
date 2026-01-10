using DevCodeX_API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevCodeX_API.Context
{
    public class CodeXContext : DbContext
    {
        public CodeXContext(DbContextOptions<CodeXContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");
        }

        public DbSet<Technology> Technology { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Asset> Asset { get; set; }
    }
}
