using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.Entities;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "User,Admin")]
	public class ShoppingController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;

		public ShoppingController(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		private async Task<int> GetUserIdFromTokenAsync()
		{
			var username = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

			if (string.IsNullOrEmpty(username))
				throw new UnauthorizedAccessException("User is not authenticated.");

			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

			if (user == null)
				throw new UnauthorizedAccessException("User not found.");

			return user.UserId;
		}

		// Generate a shopping list for a meal plan
		[HttpPost("generate")]
		public async Task<IActionResult> GenerateShoppingList(int mealPlanId)
		{
			int userId = await GetUserIdFromTokenAsync();

			var mealPlan = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
				.ThenInclude(r => r.Recipe)
				.FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

			if (mealPlan == null)
				return NotFound("Meal plan not found.");

			var ingredients = mealPlan.Recipes
				.SelectMany(r => r.Recipe.Ingredients.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
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

			return Ok(new
			{
				shoppingList.UserShoppingListId,
				shoppingList.UserId,
				shoppingList.MealPlanId,
				Items = shoppingList.Items.Select(item => new
				{
					item.UserShoppingListItemId,
					item.Ingredient,
					item.Quantity,
					item.IsPurchased
				})
			});
		}

		// Get all shopping lists for the logged-in user
		[HttpGet]
		public async Task<IActionResult> GetShoppingLists()
		{
			int userId = await GetUserIdFromTokenAsync();

			var shoppingLists = await _dbContext.UserShoppingLists
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
				})
				.ToListAsync();

			return Ok(shoppingLists);
		}

		// Mark an item as purchased
		[HttpPut("mark-purchased/{itemId}")]
		public async Task<IActionResult> MarkAsPurchased(int itemId)
		{
			int userId = await GetUserIdFromTokenAsync();

			var item = await _dbContext.UserShoppingListItems
				.Include(i => i.UserShoppingList)
				.FirstOrDefaultAsync(i => i.UserShoppingListItemId == itemId && i.UserShoppingList.UserId == userId);

			if (item == null)
				return NotFound("Item not found.");

			item.IsPurchased = true;
			await _dbContext.SaveChangesAsync();

			return Ok(new { Message = "Item marked as purchased.", ItemId = itemId });
		}

		// Unmark an item as purchased
		[HttpPut("unmark-purchased/{itemId}")]
		public async Task<IActionResult> UnmarkAsPurchased(int itemId)
		{
			int userId = await GetUserIdFromTokenAsync();

			var item = await _dbContext.UserShoppingListItems
				.Include(i => i.UserShoppingList)
				.FirstOrDefaultAsync(i => i.UserShoppingListItemId == itemId && i.UserShoppingList.UserId == userId);

			if (item == null)
				return NotFound("Item not found.");

			item.IsPurchased = false;
			await _dbContext.SaveChangesAsync();

			return Ok(new { Message = "Item unmarked as purchased.", ItemId = itemId });
		}

		// Delete an item from the shopping list
		[HttpDelete("delete-item/{itemId}")]
		public async Task<IActionResult> DeleteItem(int itemId)
		{
			int userId = await GetUserIdFromTokenAsync();

			var item = await _dbContext.UserShoppingListItems
				.Include(i => i.UserShoppingList)
				.FirstOrDefaultAsync(i => i.UserShoppingListItemId == itemId && i.UserShoppingList.UserId == userId);

			if (item == null)
				return NotFound("Item not found.");

			_dbContext.UserShoppingListItems.Remove(item);
			await _dbContext.SaveChangesAsync();

			return Ok(new { Message = "Item deleted.", ItemId = itemId });
		}

		// Delete an entire shopping list
		[HttpDelete("delete-list/{listId}")]
		public async Task<IActionResult> DeleteShoppingList(int listId)
		{
			int userId = await GetUserIdFromTokenAsync();

			var shoppingList = await _dbContext.UserShoppingLists
				.Include(sl => sl.Items)
				.FirstOrDefaultAsync(sl => sl.UserShoppingListId == listId && sl.UserId == userId);

			if (shoppingList == null)
				return NotFound("Shopping list not found.");

			// Removing the shopping list will cascade delete the items if configured in EF Core
			_dbContext.UserShoppingLists.Remove(shoppingList);
			await _dbContext.SaveChangesAsync();

			return Ok(new { Message = "Shopping list deleted.", ListId = listId });
		}
	}
}
