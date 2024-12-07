using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class ShoppingService : IShoppingService
	{
		private readonly RecipeMealDbContext _dbContext;

		public ShoppingService(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<int> GetUserIdFromTokenAsync(string username)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
			if (user == null)
				throw new UnauthorizedAccessException("User not found.");
			return user.UserId;
		}

		public async Task<UserShoppingList> GenerateShoppingListAsync(int mealPlanId, int userId)
		{
			var mealPlan = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
				.ThenInclude(r => r.Recipe)
				.FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

			if (mealPlan == null)
				throw new KeyNotFoundException("Meal plan not found.");

			var ingredients = mealPlan.Recipes
				.SelectMany(r => r.Recipe.Ingredients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				.GroupBy(ingredient => ingredient.Trim())
				.Select(g => new { Ingredient = g.Key, Quantity = g.Count() });

			var shoppingList = new UserShoppingList
			{
				UserId = userId,
				MealPlanId = mealPlanId,
				Items = ingredients.Select(i => new UserShoppingListItem
				{
					Ingredient = i.Ingredient,
					Quantity = i.Quantity,
					IsPurchased = false
				}).ToList()
			};

			await _dbContext.UserShoppingLists.AddAsync(shoppingList);
			await _dbContext.SaveChangesAsync();

			return shoppingList;
		}

		public async Task<IEnumerable<object>> GetShoppingListsAsync(int userId)
		{
			return await _dbContext.UserShoppingLists
				.Where(sl => sl.UserId == userId)
				.Include(sl => sl.Items)
				.Select(sl => new
				{
					sl.UserShoppingListId,
					sl.MealPlanId,
					Items = sl.Items.Select(i => new
					{
						i.UserShoppingListItemId,
						i.Ingredient,
						i.Quantity,
						i.IsPurchased
					})
				}).ToListAsync();
		}

		public async Task<UserShoppingListItem> MarkAsPurchasedAsync(int itemId, int userId, bool isPurchased)
		{
			var item = await _dbContext.UserShoppingListItems
				.Include(i => i.UserShoppingList)
				.FirstOrDefaultAsync(i => i.UserShoppingListItemId == itemId && i.UserShoppingList.UserId == userId);

			if (item == null)
				throw new KeyNotFoundException("Item not found.");

			item.IsPurchased = isPurchased;
			await _dbContext.SaveChangesAsync();

			return item;
		}

		public async Task<UserShoppingListItem> DeleteItemAsync(int itemId, int userId)
		{
			var item = await _dbContext.UserShoppingListItems
				.Include(i => i.UserShoppingList)
				.FirstOrDefaultAsync(i => i.UserShoppingListItemId == itemId && i.UserShoppingList.UserId == userId);

			if (item == null)
				throw new KeyNotFoundException("Item not found.");

			_dbContext.UserShoppingListItems.Remove(item);
			await _dbContext.SaveChangesAsync();

			return item;
		}

		public async Task<string> DeleteShoppingListAsync(int listId, int userId)
		{
			var shoppingList = await _dbContext.UserShoppingLists
				.Include(sl => sl.Items)
				.FirstOrDefaultAsync(sl => sl.UserShoppingListId == listId && sl.UserId == userId);

			if (shoppingList == null)
				throw new KeyNotFoundException("Shopping list not found.");

			_dbContext.UserShoppingLists.Remove(shoppingList);
			await _dbContext.SaveChangesAsync();

			return "Shopping list deleted successfully.";
		}
	}
}
