using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.Nutrition;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class NutritionService : INutritionService
	{
		private readonly RecipeMealDbContext _dbContext;

		public NutritionService(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Nutrition> AddNutritionAsync(AddNutritionDto dto)
		{
			var recipe = await _dbContext.Recipes.FindAsync(dto.RecipeId);
			if (recipe == null)
				throw new Exception("Recipe not found.");

			if (await _dbContext.Nutritions.AnyAsync(n => n.RecipeId == dto.RecipeId))
				throw new Exception("Nutrition already exists for this recipe.");

			var nutrition = new Nutrition
			{
				RecipeId = dto.RecipeId,
				Calories = dto.Calories,
				Protein = dto.Protein,
				Carbs = dto.Carbs,
				Fat = dto.Fat,
				Vitamins = dto.Vitamins
			};

			_dbContext.Nutritions.Add(nutrition);
			await _dbContext.SaveChangesAsync();

			return nutrition;
		}

		public async Task<Nutrition> UpdateNutritionAsync(int nutritionId, UpdateNutritionDto dto)
		{
			var nutrition = await _dbContext.Nutritions.FindAsync(nutritionId);
			if (nutrition == null)
				throw new Exception("Nutrition data not found.");

			nutrition.Calories = dto.Calories;
			nutrition.Protein = dto.Protein;
			nutrition.Carbs = dto.Carbs;
			nutrition.Fat = dto.Fat;
			nutrition.Vitamins = dto.Vitamins;

			_dbContext.Nutritions.Update(nutrition);
			await _dbContext.SaveChangesAsync();

			return nutrition;
		}

		public async Task<Nutrition> PatchNutritionAsync(int nutritionId, PatchNutritionDto dto)
		{
			var nutrition = await _dbContext.Nutritions.FindAsync(nutritionId);
			if (nutrition == null)
				throw new Exception("Nutrition data not found.");

			if (dto.Calories.HasValue) nutrition.Calories = dto.Calories.Value;
			if (dto.Protein.HasValue) nutrition.Protein = dto.Protein.Value;
			if (dto.Carbs.HasValue) nutrition.Carbs = dto.Carbs.Value;
			if (dto.Fat.HasValue) nutrition.Fat = dto.Fat.Value;
			if (!string.IsNullOrWhiteSpace(dto.Vitamins)) nutrition.Vitamins = dto.Vitamins;

			_dbContext.Nutritions.Update(nutrition);
			await _dbContext.SaveChangesAsync();

			return nutrition;
		}

		public async Task<IEnumerable<object>> GetPendingMealsAsync()
		{
			return await _dbContext.Recipes
				.Where(r => !_dbContext.Nutritions.Any(n => n.RecipeId == r.RecipeId))
				.Select(r => new
				{
					r.RecipeId,
					r.Title,
					r.Description,
					r.Category,
					r.CreatedBy,
					r.CreatedAt
				}).ToListAsync();
		}

		public async Task<IEnumerable<object>> GetAllNutritionAsync()
		{
			return await _dbContext.Nutritions
				.Include(n => n.Recipe)
				.Select(n => new
				{
					n.NutritionId,
					n.RecipeId,
					RecipeTitle = n.Recipe.Title,
					n.Calories,
					n.Protein,
					n.Carbs,
					n.Fat,
					n.Vitamins
				}).ToListAsync();
		}

		public async Task<Nutrition> GetNutritionByRecipeAsync(int recipeId)
		{
			var nutrition = await _dbContext.Nutritions
				.Include(n => n.Recipe)
				.FirstOrDefaultAsync(n => n.RecipeId == recipeId);

			if (nutrition == null)
				throw new Exception("Nutrition data not found for the specified recipe.");

			return nutrition;
		}

		public async Task<string> DeleteNutritionByRecipeIdAsync(int recipeId)
		{
			var nutrition = await _dbContext.Nutritions.FirstOrDefaultAsync(n => n.RecipeId == recipeId);
			if (nutrition == null)
				throw new Exception($"No nutrition entry found for Recipe ID {recipeId}.");

			_dbContext.Nutritions.Remove(nutrition);
			await _dbContext.SaveChangesAsync();

			return $"Nutrition entry for Recipe ID {recipeId} deleted successfully.";
		}
	}
}
