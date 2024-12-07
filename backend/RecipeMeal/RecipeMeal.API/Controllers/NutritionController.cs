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

			if (await _dbContext.Nutritions.AnyAsync(n => n.RecipeId == dto.RecipeId))
				return BadRequest("Nutrition already exists for this recipe.");

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

			nutrition.Calories = dto.Calories;
			nutrition.Protein = dto.Protein;
			nutrition.Carbs = dto.Carbs;
			nutrition.Fat = dto.Fat;
			nutrition.Vitamins = dto.Vitamins;

			_dbContext.Nutritions.Update(nutrition);
			await _dbContext.SaveChangesAsync();

			return Ok(nutrition);
		}

		// Patch Nutrition
		[HttpPatch("{nutritionId}")]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> PatchNutrition(int nutritionId, [FromBody] PatchNutritionDto dto)
		{
			var nutrition = await _dbContext.Nutritions.FindAsync(nutritionId);
			if (nutrition == null)
				return NotFound("Nutrition data not found.");

			if (dto.Calories.HasValue) nutrition.Calories = dto.Calories.Value;
			if (dto.Protein.HasValue) nutrition.Protein = dto.Protein.Value;
			if (dto.Carbs.HasValue) nutrition.Carbs = dto.Carbs.Value;
			if (dto.Fat.HasValue) nutrition.Fat = dto.Fat.Value;
			if (!string.IsNullOrWhiteSpace(dto.Vitamins)) nutrition.Vitamins = dto.Vitamins;

			_dbContext.Nutritions.Update(nutrition);
			await _dbContext.SaveChangesAsync();

			return Ok(nutrition);
		}

		// Get Pending Meals
		[HttpGet("pending")]
		[Authorize(Roles = "Nutritionist,Admin")]
		public IActionResult GetPendingMeals()
		{
			var pendingMeals = _dbContext.Recipes
				.Where(r => !_dbContext.Nutritions.Any(n => n.RecipeId == r.RecipeId))
				.Select(r => new
				{
					r.RecipeId,
					r.Title,
					r.Description,
					r.Category,
					r.CreatedBy,
					r.CreatedAt
				}).ToList();

			return Ok(pendingMeals);
		}

		// Get All Nutrition Data
		[HttpGet]
		[Authorize(Roles = "Nutritionist,Admin")]
		public IActionResult GetAllNutrition()
		{
			var nutritions = _dbContext.Nutritions
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
				}).ToList();

			return Ok(nutritions);
		}

		// Get Nutrition By Recipe
		[HttpGet("recipe/{recipeId}")]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> GetNutritionByRecipe(int recipeId)
		{
			var nutrition = await _dbContext.Nutritions
				.Include(n => n.Recipe)
				.FirstOrDefaultAsync(n => n.RecipeId == recipeId);

			if (nutrition == null)
				return NotFound("Nutrition data not found for the specified recipe.");

			return Ok(nutrition);
		}

		[HttpDelete("recipe/{recipeId}")]
		[Authorize(Roles = "Nutritionist,Admin")]
		public async Task<IActionResult> DeleteNutritionByRecipeId(int recipeId)
		{
			// Find the nutrition entry based on the RecipeId
			var nutrition = await _dbContext.Nutritions.FirstOrDefaultAsync(n => n.RecipeId == recipeId);

			if (nutrition == null)
			{
				return NotFound($"No nutrition entry found for Recipe ID {recipeId}.");
			}

			// Remove the nutrition entry
			_dbContext.Nutritions.Remove(nutrition);
			await _dbContext.SaveChangesAsync();

			return Ok($"Nutrition entry for Recipe ID {recipeId} deleted successfully.");
		}
	}

}
