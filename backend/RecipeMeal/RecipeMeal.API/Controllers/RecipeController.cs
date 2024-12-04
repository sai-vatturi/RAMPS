using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.Recipe;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces;
using RecipeMeal.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RecipeController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;
		private readonly IImageService _imageService;

		public RecipeController(RecipeMealDbContext dbContext, IImageService imageService)
		{
			_dbContext = dbContext;
			_imageService = imageService;
		}

		// Create a new recipe
		[HttpPost]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> CreateRecipe([FromForm] CreateRecipeDto dto)
		{
			// Upload image to Azure Blob Storage
			string imageUrl = await _imageService.UploadImageAsync(dto.Image);

			var recipe = new Recipe
			{
				Title = dto.Title,
				Description = dto.Description,
				Ingredients = dto.Ingredients,
				Steps = dto.Steps,
				Category = dto.Category,
				ImageUrl = imageUrl,
				CreatedBy = HttpContext.User.Identity.Name,
				CreatedAt = DateTime.UtcNow
			};

			_dbContext.Recipes.Add(recipe);
			await _dbContext.SaveChangesAsync();

			return Ok(recipe);
		}

		// Update an existing recipe
		[HttpPut("{id}")]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> UpdateRecipe(int id, [FromForm] UpdateRecipeDto dto)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);
			if (recipe == null)
				return NotFound("Recipe not found.");

			// Ownership check
			if (recipe.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to update this recipe.");

			recipe.Title = dto.Title ?? recipe.Title;
			recipe.Description = dto.Description ?? recipe.Description;
			recipe.Ingredients = dto.Ingredients ?? recipe.Ingredients;
			recipe.Steps = dto.Steps ?? recipe.Steps;
			recipe.Category = dto.Category ?? recipe.Category;

			if (dto.Image != null)
			{
				// Upload the new image and update the URL
				recipe.ImageUrl = await _imageService.UploadImageAsync(dto.Image);
			}

			recipe.UpdatedAt = DateTime.UtcNow;

			_dbContext.Recipes.Update(recipe);
			await _dbContext.SaveChangesAsync();

			return Ok(recipe);
		}

		// Get all recipes
		[HttpGet]
		[AllowAnonymous]
		public IActionResult GetAllRecipes()
		{
			var recipes = _dbContext.Recipes
				.Select(r => new
				{
					r.RecipeId,
					r.Title,
					r.Description,
					r.Category,
					r.ImageUrl,
					r.CreatedBy,
					r.CreatedAt
				})
				.ToList();

			return Ok(recipes);
		}

		// Get a specific recipe by ID
		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetRecipeById(int id)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);

			if (recipe == null)
				return NotFound("Recipe not found.");

			return Ok(recipe);
		}

		// Delete a recipe
		[HttpDelete("{id}")]
		[Authorize(Roles = "Chef,Admin")]
		public async Task<IActionResult> DeleteRecipe(int id)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);

			if (recipe == null)
				return NotFound("Recipe not found.");

			// Ownership check
			if (recipe.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to delete this recipe.");

			_dbContext.Recipes.Remove(recipe);
			await _dbContext.SaveChangesAsync();

			return Ok("Recipe deleted successfully.");
		}
	}
}
