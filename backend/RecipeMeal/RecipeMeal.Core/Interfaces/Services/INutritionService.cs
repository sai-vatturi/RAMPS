using RecipeMeal.Core.DTOs.Nutrition;
using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface INutritionService
	{
		Task<Nutrition> AddNutritionAsync(AddNutritionDto dto);
		Task<Nutrition> UpdateNutritionAsync(int nutritionId, UpdateNutritionDto dto);
		Task<Nutrition> PatchNutritionAsync(int nutritionId, PatchNutritionDto dto);
		Task<IEnumerable<object>> GetPendingMealsAsync();
		Task<IEnumerable<object>> GetAllNutritionAsync();
		Task<Nutrition> GetNutritionByRecipeAsync(int recipeId);
		Task<string> DeleteNutritionByRecipeIdAsync(int recipeId);
	}
}
