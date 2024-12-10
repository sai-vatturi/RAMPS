using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.MealPlan;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class MealPlanService : IMealPlanService
	{
		private readonly RecipeMealDbContext _dbContext;

		public MealPlanService(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<MealPlan> CreateMealPlanAsync(CreateMealPlanDto dto, string createdBy)
		{
			var mealPlan = new MealPlan
			{
				Name = dto.Name,
				CreatedBy = createdBy,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				Recipes = dto.Recipes.Select(r => new MealPlanRecipe
				{
					RecipeId = r.RecipeId,
					MealTime = r.MealTime
				}).ToList()
			};

			_dbContext.MealPlans.Add(mealPlan);
			await _dbContext.SaveChangesAsync();
			return mealPlan;
		}

		public async Task<MealPlan> UpdateMealPlanAsync(int id, CreateMealPlanDto dto, string updatedBy)
		{
			var mealPlan = await _dbContext.MealPlans.Include(mp => mp.Recipes).FirstOrDefaultAsync(mp => mp.MealPlanId == id);
			if (mealPlan == null)
				throw new Exception("Meal Plan not found.");

			mealPlan.Name = dto.Name;
			mealPlan.StartDate = dto.StartDate;
			mealPlan.EndDate = dto.EndDate;
			mealPlan.Recipes = dto.Recipes.Select(r => new MealPlanRecipe
			{
				RecipeId = r.RecipeId,
				MealTime = r.MealTime
			}).ToList();

			_dbContext.MealPlans.Update(mealPlan);
			await _dbContext.SaveChangesAsync();
			return mealPlan;
		}

		public async Task<IEnumerable<object>> GetMealPlansAsync()
		{
			return await _dbContext.MealPlans
				.Select(mp => new
				{
					mp.MealPlanId,
					mp.Name,
					mp.StartDate,
					mp.EndDate,
					Recipes = mp.Recipes.Select(r => new
					{
						r.RecipeId,
						r.Recipe.Title,
						r.MealTime
					})
				}).ToListAsync();
		}


		public async Task<object> GetMealPlanByIdAsync(int id, string username, bool isAdmin)
		{
			var mealPlan = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
				.ThenInclude(r => r.Recipe)
				.FirstOrDefaultAsync(mp => mp.MealPlanId == id);

			if (mealPlan == null)
				throw new Exception("Meal Plan not found.");

			if (mealPlan.CreatedBy != username && !isAdmin)
				throw new UnauthorizedAccessException("You are not authorized to view this meal plan.");

			return new
			{
				mealPlan.MealPlanId,
				mealPlan.Name,
				mealPlan.StartDate,
				mealPlan.EndDate,
				Recipes = mealPlan.Recipes.Select(r => new
				{
					r.RecipeId,
					r.Recipe.Title,
					r.MealTime
				})
			};
		}

		public async Task<string> DeleteMealPlanAsync(int id, string deletedBy, bool isAdmin)
		{
			var mealPlan = await _dbContext.MealPlans.FindAsync(id);
			if (mealPlan == null)
				throw new Exception("Meal Plan not found.");

			if (mealPlan.CreatedBy != deletedBy && !isAdmin)
				throw new UnauthorizedAccessException("You are not authorized to delete this meal plan.");

			_dbContext.MealPlans.Remove(mealPlan);
			await _dbContext.SaveChangesAsync();
			return "Meal Plan deleted successfully.";
		}

		public async Task<MealPlan> PatchMealPlanAsync(int id, PatchMealPlanDto dto, string updatedBy, bool isAdmin)
		{
			var mealPlan = await _dbContext.MealPlans.Include(mp => mp.Recipes).FirstOrDefaultAsync(mp => mp.MealPlanId == id);
			if (mealPlan == null)
				throw new Exception("Meal Plan not found.");

			if (mealPlan.CreatedBy != updatedBy && !isAdmin)
				throw new UnauthorizedAccessException("You are not authorized to update this meal plan.");

			if (!string.IsNullOrEmpty(dto.Name))
				mealPlan.Name = dto.Name;
			if (dto.StartDate.HasValue)
				mealPlan.StartDate = dto.StartDate.Value;
			if (dto.EndDate.HasValue)
				mealPlan.EndDate = dto.EndDate.Value;

			if (dto.Recipes != null && dto.Recipes.Any())
			{
				foreach (var recipeDto in dto.Recipes)
				{
					if (recipeDto.RecipeId.HasValue && recipeDto.MealTime.HasValue)
					{
						var existingRecipe = mealPlan.Recipes.FirstOrDefault(r => r.RecipeId == recipeDto.RecipeId.Value);
						if (existingRecipe != null)
						{
							existingRecipe.MealTime = recipeDto.MealTime.Value;
						}
						else
						{
							mealPlan.Recipes.Add(new MealPlanRecipe
							{
								RecipeId = recipeDto.RecipeId.Value,
								MealTime = recipeDto.MealTime.Value
							});
						}
					}
				}
			}

			_dbContext.MealPlans.Update(mealPlan);
			await _dbContext.SaveChangesAsync();
			return mealPlan;
		}
	}
}
