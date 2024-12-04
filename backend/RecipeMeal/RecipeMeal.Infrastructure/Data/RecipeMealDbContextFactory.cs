using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RecipeMeal.Infrastructure.Data
{
    public class RecipeMealDbContextFactory : IDesignTimeDbContextFactory<RecipeMealDbContext>
    {
        public RecipeMealDbContext CreateDbContext(string[] args)
        {
            // Locate the appsettings.json in the API project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RecipeMeal.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RecipeMealDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new RecipeMealDbContext(optionsBuilder.Options);
        }
    }
}
