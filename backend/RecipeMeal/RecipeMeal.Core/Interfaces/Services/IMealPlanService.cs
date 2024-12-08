using RecipeMeal.Core.DTOs.MealPlan;
using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IMealPlanService
	{
		Task<MealPlan> CreateMealPlanAsync(CreateMealPlanDto dto, string createdBy);
		Task<MealPlan> UpdateMealPlanAsync(int id, CreateMealPlanDto dto, string updatedBy);
		Task<IEnumerable<object>> GetMealPlansAsync();
		Task<object> GetMealPlanByIdAsync(int id, string username, bool isAdmin);
		Task<string> DeleteMealPlanAsync(int id, string deletedBy, bool isAdmin);
		Task<MealPlan> PatchMealPlanAsync(int id, PatchMealPlanDto dto, string updatedBy, bool isAdmin);
	}
}
