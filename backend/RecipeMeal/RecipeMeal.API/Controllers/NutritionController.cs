using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.Nutrition;
using RecipeMeal.Core.Entities;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NutritionController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;

		public NutritionController(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// Add Nutrition
		[HttpPost]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> AddNutrition([FromBody] AddNutritionDto dto)
		{
			var recipe = await _dbContext.Recipes.FindAsync(dto.RecipeId);
			if (recipe == null)
				return NotFound("Recipe not found.");

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

			return Ok(nutrition);
		}

		// Update Nutrition
		[HttpPut("{nutritionId}")]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> UpdateNutrition(int nutritionId, [FromBody] UpdateNutritionDto dto)
		{
			var nutrition = await _dbContext.Nutritions.FindAsync(nutritionId);
			if (nutrition == null)
				return NotFound("Nutrition data not found.");

			// Check ownership or admin override
			var recipe = await _dbContext.Recipes.FindAsync(nutrition.RecipeId);
			var createdBy = recipe?.CreatedBy;

			if (createdBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to update this nutrition data.");

			nutrition.Calories = dto.Calories;
			nutrition.Protein = dto.Protein;
			nutrition.Carbs = dto.Carbs;
			nutrition.Fat = dto.Fat;
			nutrition.Vitamins = dto.Vitamins;

			_dbContext.Nutritions.Update(nutrition);
			await _dbContext.SaveChangesAsync();

			return Ok(nutrition);
		}

		// Get Nutrition Summary for Meal Plan
		[HttpGet("meal-plan/{mealPlanId}/summary")]
		[Authorize(Roles = "MealPlanner,User,Admin")]
		public async Task<IActionResult> GetMealPlanNutritionSummary(int mealPlanId)
		{
			var mealPlan = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
				.ThenInclude(mpr => mpr.Recipe)
				.ThenInclude(r => r.Nutrition)
				.FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

			if (mealPlan == null)
				return NotFound("Meal plan not found.");

			var summary = mealPlan.Recipes
				.Select(r => r.Recipe.Nutrition)
				.GroupBy(n => true)
				.Select(group => new
				{
					Calories = group.Sum(n => n.Calories),
					Protein = group.Sum(n => n.Protein),
					Carbs = group.Sum(n => n.Carbs),
					Fat = group.Sum(n => n.Fat),
					Vitamins = string.Join(", ", group.Select(n => n.Vitamins).Where(v => !string.IsNullOrEmpty(v)))
				}).FirstOrDefault();

			return Ok(summary);
		}
	}
}
