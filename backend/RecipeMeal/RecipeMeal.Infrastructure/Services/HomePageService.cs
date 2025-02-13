using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.HomePage;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using RecipeMeal.Core.Exceptions;

namespace RecipeMeal.Infrastructure.Services
{
	public class HomePageService : IHomePageService
	{
		private readonly RecipeMealDbContext _dbContext;

		public HomePageService(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<HomePageResponseDto> GetCurrentMealPlanAsync(DateTime date)
		{
			var mealPlans = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
					.ThenInclude(r => r.Recipe)
						.ThenInclude(r => r.Nutrition)
				.Where(mp => mp.StartDate.Date <= date.Date && mp.EndDate.Date >= date.Date)
				.ToListAsync();

			if (!mealPlans.Any())
				throw new KeyNotFoundException($"No meal plan found for date {date.Date:yyyy-MM-dd}");

			if (mealPlans.Count > 1)
				throw new MultipleMealPlansException(date);

			var mealPlan = mealPlans.First();

			// Get all recipes for this meal plan
			var recipes = mealPlan.Recipes
				.Where(r => r.Recipe != null)  // Ensure recipe exists
				.Select(r => new
				{
					r.MealTime.Date,
					r.Recipe,
					r.MealTime
				})
				.ToList();

			if (!recipes.Any())
				throw new KeyNotFoundException($"No recipes found in meal plan {mealPlan.MealPlanId}");

			return await CreateHomePageResponseDto(mealPlan, date);
		}

		public async Task<List<HomePageResponseDto>> GetMealPlansByDateRangeAsync(DateTime startDate, DateTime endDate)
		{
			var mealPlans = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
					.ThenInclude(r => r.Recipe)
						.ThenInclude(r => r.Nutrition)
				.Where(mp => (mp.StartDate <= endDate && mp.EndDate >= startDate))
				.ToListAsync();

			var response = new List<HomePageResponseDto>();
			foreach (var mealPlan in mealPlans)
			{
				response.Add(await CreateHomePageResponseDto(mealPlan, DateTime.Today));
			}

			return response;
		}

		private async Task<HomePageResponseDto> CreateHomePageResponseDto(MealPlan mealPlan, DateTime date)
		{
			var dailyMeals = mealPlan.Recipes
				.Where(r => r.Recipe != null)  // Ensure recipe exists
				.Select(r => new DailyMealDto
				{
					RecipeId = r.RecipeId,
					RecipeTitle = r.Recipe.Title,
					ImageUrl = r.Recipe.ImageUrl ?? "",
					MealTime = r.MealTime,
					RecipeDetails = new RecipeDetailDto
					{
						Description = r.Recipe.Description ?? "",
						Ingredients = r.Recipe.Ingredients ?? "",
						Steps = r.Recipe.Steps ?? "",
						Nutrition = r.Recipe.Nutrition != null ? new NutritionInfoDto
						{
							Calories = r.Recipe.Nutrition.Calories,
							Protein = r.Recipe.Nutrition.Protein,
							Carbs = r.Recipe.Nutrition.Carbs,
							Fat = r.Recipe.Nutrition.Fat,
							Vitamins = r.Recipe.Nutrition.Vitamins ?? ""
						} : new NutritionInfoDto() // Default values if no nutrition data
					}
				})
				.ToList();

			var dailyNutrition = new DailyNutritionSummaryDto
			{
				TotalCalories = dailyMeals.Sum(m => m.RecipeDetails?.Nutrition?.Calories ?? 0),
				TotalProtein = dailyMeals.Sum(m => m.RecipeDetails?.Nutrition?.Protein ?? 0),
				TotalCarbs = dailyMeals.Sum(m => m.RecipeDetails?.Nutrition?.Carbs ?? 0),
				TotalFat = dailyMeals.Sum(m => m.RecipeDetails?.Nutrition?.Fat ?? 0)
			};

			return new HomePageResponseDto
			{
				MealPlanId = mealPlan.MealPlanId,
				MealPlanName = mealPlan.Name ?? "Untitled Meal Plan",
				DailyMeals = dailyMeals,
				DailyNutrition = dailyNutrition
			};
		}
	}
}
