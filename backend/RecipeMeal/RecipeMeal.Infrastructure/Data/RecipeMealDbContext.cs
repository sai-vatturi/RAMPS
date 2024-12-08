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
		public DbSet<UserShoppingList> UserShoppingLists { get; set; }
		public DbSet<UserShoppingListItem> UserShoppingListItems { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Nutrition> Nutritions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.Property(u => u.Role)
				.HasConversion<string>();

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

			modelBuilder.Entity<UserShoppingList>()
				.HasMany(sl => sl.Items)
				.WithOne(i => i.UserShoppingList)
				.HasForeignKey(i => i.UserShoppingListId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserShoppingListItem>()
				.Property(usi => usi.Ingredient)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Entity<UserShoppingListItem>()
				.Property(usi => usi.Quantity)
				.HasDefaultValue(1);
		}
	}
}
