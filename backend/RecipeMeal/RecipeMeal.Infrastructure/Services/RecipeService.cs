using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.Recipe;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly RecipeMealDbContext _dbContext;
		private readonly IImageService _imageService;

		public RecipeService(RecipeMealDbContext dbContext, IImageService imageService)
		{
			_dbContext = dbContext;
			_imageService = imageService;
		}

		public async Task<Recipe> CreateRecipeAsync(CreateRecipeDto dto, string createdBy)
		{
			var imageUrl = await _imageService.UploadImageAsync(dto.Image);

			var recipe = new Recipe
			{
				Title = dto.Title,
				Description = dto.Description,
				Ingredients = dto.Ingredients,
				Steps = dto.Steps,
				Category = dto.Category,
				ImageUrl = imageUrl,
				CreatedBy = createdBy,
				CreatedAt = DateTime.UtcNow
			};

			_dbContext.Recipes.Add(recipe);
			await _dbContext.SaveChangesAsync();

			return recipe;
		}

		public async Task<Recipe> UpdateRecipeAsync(int id, UpdateRecipeDto dto, string updatedBy)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);
			if (recipe == null)
				throw new Exception("Recipe not found.");

			if (recipe.CreatedBy != updatedBy && !await IsUserAdmin(updatedBy))
				throw new UnauthorizedAccessException("You are not authorized to update this recipe.");

			recipe.Title = dto.Title ?? recipe.Title;
			recipe.Description = dto.Description ?? recipe.Description;
			recipe.Ingredients = dto.Ingredients ?? recipe.Ingredients;
			recipe.Steps = dto.Steps ?? recipe.Steps;
			recipe.Category = dto.Category ?? recipe.Category;

			if (dto.Image != null)
				recipe.ImageUrl = await _imageService.UploadImageAsync(dto.Image);

			recipe.UpdatedAt = DateTime.UtcNow;

			_dbContext.Recipes.Update(recipe);
			await _dbContext.SaveChangesAsync();

			return recipe;
		}

		public async Task<IEnumerable<object>> GetAllRecipesAsync()
		{
			return await _dbContext.Recipes
				.Select(r => new
				{
					r.RecipeId,
					r.Title,
					r.Description,
					r.Category,
					r.ImageUrl,
					r.Steps,
					r.Ingredients,
					r.CreatedBy,
					r.CreatedAt
				})
				.ToListAsync();
		}

		public async Task<Recipe> GetRecipeByIdAsync(int id)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);
			if (recipe == null)
				throw new Exception("Recipe not found.");
			return recipe;
		}

		public async Task<string> DeleteRecipeAsync(int id, string deletedBy)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);
			if (recipe == null)
				throw new Exception("Recipe not found.");

			if (recipe.CreatedBy != deletedBy && !await IsUserAdmin(deletedBy))
				throw new UnauthorizedAccessException("You are not authorized to delete this recipe.");

			_dbContext.Recipes.Remove(recipe);
			await _dbContext.SaveChangesAsync();

			return "Recipe deleted successfully.";
		}

		public async Task<Recipe> PatchRecipeAsync(int id, PatchRecipeDto dto, string updatedBy)
		{
			var recipe = await _dbContext.Recipes.FindAsync(id);
			if (recipe == null)
				throw new Exception("Recipe not found.");

			if (recipe.CreatedBy != updatedBy && !await IsUserAdmin(updatedBy))
				throw new UnauthorizedAccessException("You are not authorized to update this recipe.");

			if (!string.IsNullOrEmpty(dto.Title))
				recipe.Title = dto.Title;
			if (!string.IsNullOrEmpty(dto.Description))
				recipe.Description = dto.Description;
			if (!string.IsNullOrEmpty(dto.Ingredients))
				recipe.Ingredients = dto.Ingredients;
			if (!string.IsNullOrEmpty(dto.Steps))
				recipe.Steps = dto.Steps;
			if (!string.IsNullOrEmpty(dto.Category))
				recipe.Category = dto.Category;

			if (dto.Image != null)
				recipe.ImageUrl = await _imageService.UploadImageAsync(dto.Image);

			recipe.UpdatedAt = DateTime.UtcNow;

			_dbContext.Recipes.Update(recipe);
			await _dbContext.SaveChangesAsync();

			return recipe;
		}

		private async Task<bool> IsUserAdmin(string username)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
			return user?.Role == Core.Enums.Role.Admin;
		}
	}
}
