using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IShoppingService
	{
		Task<int> GetUserIdFromTokenAsync(string username);
		Task<string> GetUserEmailAsync(int userId); // New method
		Task<UserShoppingList> GenerateShoppingListAsync(int mealPlanId, int userId);
		Task<IEnumerable<object>> GetShoppingListsAsync(int userId);
		Task<UserShoppingListItem> MarkAsPurchasedAsync(int itemId, int userId, bool isPurchased);
		Task<UserShoppingListItem> DeleteItemAsync(int itemId, int userId);
		Task<string> DeleteShoppingListAsync(int listId, int userId);
	}
}
