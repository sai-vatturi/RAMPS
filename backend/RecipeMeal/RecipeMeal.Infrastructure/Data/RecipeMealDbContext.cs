using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.Entities;

namespace RecipeMeal.Infrastructure.Data
{
    public class RecipeMealDbContext : DbContext
    {
        public RecipeMealDbContext(DbContextOptions<RecipeMealDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); // Store the Role enum as a string in the database
        }
    }
}
