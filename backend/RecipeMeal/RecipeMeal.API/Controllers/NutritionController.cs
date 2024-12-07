using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs.Nutrition;
using RecipeMeal.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NutritionController : ControllerBase
	{
		private readonly INutritionService _nutritionService;

		public NutritionController(INutritionService nutritionService)
		{
			_nutritionService = nutritionService;
		}

		[HttpPost]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> AddNutrition([FromBody] AddNutritionDto dto)
		{
			try
			{
				var nutrition = await _nutritionService.AddNutritionAsync(dto);
				return Ok(nutrition);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{nutritionId}")]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> UpdateNutrition(int nutritionId, [FromBody] UpdateNutritionDto dto)
		{
			try
			{
				var nutrition = await _nutritionService.UpdateNutritionAsync(nutritionId, dto);
				return Ok(nutrition);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpPatch("{nutritionId}")]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> PatchNutrition(int nutritionId, [FromBody] PatchNutritionDto dto)
		{
			try
			{
				var nutrition = await _nutritionService.PatchNutritionAsync(nutritionId, dto);
				return Ok(nutrition);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpGet("pending")]
		[Authorize(Roles = "Nutritionist,Admin")]
		public async Task<IActionResult> GetPendingMeals()
		{
			var pendingMeals = await _nutritionService.GetPendingMealsAsync();
			return Ok(pendingMeals);
		}

		[HttpGet]
		[Authorize(Roles = "Nutritionist,Admin")]
		public async Task<IActionResult> GetAllNutrition()
		{
			var nutritions = await _nutritionService.GetAllNutritionAsync();
			return Ok(nutritions);
		}

		[HttpGet("recipe/{recipeId}")]
		[Authorize(Roles = "Nutritionist,Chef,Admin")]
		public async Task<IActionResult> GetNutritionByRecipe(int recipeId)
		{
			try
			{
				var nutrition = await _nutritionService.GetNutritionByRecipeAsync(recipeId);
				return Ok(nutrition);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpDelete("recipe/{recipeId}")]
		[Authorize(Roles = "Nutritionist,Admin")]
		public async Task<IActionResult> DeleteNutritionByRecipeId(int recipeId)
		{
			try
			{
				var message = await _nutritionService.DeleteNutritionByRecipeIdAsync(recipeId);
				return Ok(new { message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}
	}
}
