using RecipeMeal.Core.DTOs.Recipe;
using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IRecipeService
	{
		Task<Recipe> CreateRecipeAsync(CreateRecipeDto dto, string createdBy);
		Task<Recipe> UpdateRecipeAsync(int id, UpdateRecipeDto dto, string updatedBy);
		Task<IEnumerable<object>> GetAllRecipesAsync(int pageNumber, int pageSize);
		Task<Recipe> GetRecipeByIdAsync(int id);
		Task<int> GetRecipeCountAsync();
		Task<string> DeleteRecipeAsync(int id, string deletedBy);
		Task<Recipe> PatchRecipeAsync(int id, PatchRecipeDto dto, string updatedBy);
	}
}
