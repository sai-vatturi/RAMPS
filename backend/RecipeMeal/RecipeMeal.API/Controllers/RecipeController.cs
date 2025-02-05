using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs.Recipe;
using RecipeMeal.Core.Interfaces.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RecipeController : ControllerBase
	{
		private readonly IRecipeService _recipeService;

		public RecipeController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[HttpPost]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> CreateRecipe([FromForm] CreateRecipeDto dto)
		{
			try
			{
				var createdBy = User.FindFirstValue(ClaimTypes.Name);
				var recipe = await _recipeService.CreateRecipeAsync(dto, createdBy);
				return Ok(recipe);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> UpdateRecipe(int id, [FromForm] UpdateRecipeDto dto)
		{
			try
			{
				var updatedBy = User.FindFirstValue(ClaimTypes.Name);
				var recipe = await _recipeService.UpdateRecipeAsync(id, dto, updatedBy);
				return Ok(recipe);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllRecipes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			try
			{
				var recipes = await _recipeService.GetAllRecipesAsync(pageNumber, pageSize);
				return Ok(recipes);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("count")]
		[AllowAnonymous]
		public async Task<IActionResult> GetRecipeCount()
		{
			try
			{
				var count = await _recipeService.GetRecipeCountAsync();
				return Ok(new { count });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetRecipeById(int id)
		{
			try
			{
				var recipe = await _recipeService.GetRecipeByIdAsync(id);
				return Ok(recipe);
			}
			catch (Exception ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> DeleteRecipe(int id)
		{
			try
			{
				var deletedBy = User.FindFirstValue(ClaimTypes.Name);
				var message = await _recipeService.DeleteRecipeAsync(id, deletedBy);
				return Ok(new { message });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}
		[HttpPatch("{id}")]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> PatchRecipe(int id, [FromForm] PatchRecipeDto dto)
		{
			try
			{
				var updatedBy = User.FindFirstValue(ClaimTypes.Name);
				var recipe = await _recipeService.PatchRecipeAsync(id, dto, updatedBy);
				return Ok(recipe);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
