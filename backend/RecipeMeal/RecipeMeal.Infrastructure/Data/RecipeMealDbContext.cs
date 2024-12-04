using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.Entities;

namespace RecipeMeal.Infrastructure.Data
{
	public class RecipeMealDbContext : DbContext
	{
		public RecipeMealDbContext(DbContextOptions<RecipeMealDbContext> options) : base(options) { }

		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<MealPlan> MealPlans { get; set; }
		public DbSet<MealPlanRecipe> MealPlanRecipes { get; set; }
		public DbSet<ShoppingList> ShoppingLists { get; set; }
		public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
		public DbSet<Review> Reviews { get; set; } // Include Reviews entity
		public DbSet<Nutrition> Nutritions { get; set; } // Add this line

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User entity configuration
			modelBuilder.Entity<User>()
				.Property(u => u.Role)
				.HasConversion<string>(); // Store the Role enum as a string in the database

			modelBuilder.Entity<User>()
				.Property(u => u.Username)
				.IsRequired()
				.HasMaxLength(50);

			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Entity<User>()
				.Property(u => u.PasswordHash)
				.IsRequired();

			// Recipe entity configuration
			modelBuilder.Entity<Recipe>()
				.Property(r => r.Title)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Entity<Recipe>()
				.Property(r => r.Description)
				.IsRequired();

			modelBuilder.Entity<Recipe>()
				.Property(r => r.Category)
				.HasMaxLength(50);

			modelBuilder.Entity<Recipe>()
				.Property(r => r.CreatedAt)
				.HasDefaultValueSql("GETUTCDATE()");

			modelBuilder.Entity<Recipe>()
				.HasMany(r => r.Reviews)
				.WithOne(rv => rv.Recipe)
				.HasForeignKey(rv => rv.RecipeId);

			modelBuilder.Entity<Recipe>()
				.HasOne(r => r.Nutrition)
				.WithOne(n => n.Recipe)
				.HasForeignKey<Nutrition>(n => n.RecipeId)
				.OnDelete(DeleteBehavior.Cascade);

			// MealPlanRecipe configuration (junction table)
			modelBuilder.Entity<MealPlanRecipe>()
				.HasKey(mp => new { mp.MealPlanId, mp.RecipeId });

			modelBuilder.Entity<MealPlanRecipe>()
				.HasOne(mp => mp.MealPlan)
				.WithMany(m => m.Recipes)
				.HasForeignKey(mp => mp.MealPlanId);

			modelBuilder.Entity<MealPlanRecipe>()
				.HasOne(mp => mp.Recipe)
				.WithMany()
				.HasForeignKey(mp => mp.RecipeId);

			// ShoppingList configuration
			modelBuilder.Entity<ShoppingList>()
				.HasMany(sl => sl.Items)
				.WithOne(i => i.ShoppingList)
				.HasForeignKey(i => i.ShoppingListId);

			// ShoppingListItem configuration
			modelBuilder.Entity<ShoppingListItem>()
				.Property(i => i.Ingredient)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Entity<ShoppingListItem>()
				.Property(i => i.Quantity)
				.HasDefaultValue(1);

			// Review entity configuration
			modelBuilder.Entity<Review>()
				.Property(rv => rv.UserName)
				.IsRequired();

			modelBuilder.Entity<Review>()
				.Property(rv => rv.Rating)
				.IsRequired();

			modelBuilder.Entity<Review>()
				.Property(rv => rv.Comment)
				.HasMaxLength(500);

			modelBuilder.Entity<Review>()
				.Property(rv => rv.CreatedAt)
				.HasDefaultValueSql("GETUTCDATE()");
		}
	}
}
