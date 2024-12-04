using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.ShoppingList;
using RecipeMeal.Core.Entities;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ShoppingListController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;

		public ShoppingListController(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// Generate shopping list for a meal plan
		[HttpPost("generate")]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> GenerateShoppingList([FromBody] CreateShoppingListDto dto)
		{
			var mealPlan = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
				.ThenInclude(mpr => mpr.Recipe)
				.FirstOrDefaultAsync(mp => mp.MealPlanId == dto.MealPlanId);

			if (mealPlan == null)
				return NotFound("Meal plan not found.");

			var shoppingList = new ShoppingList
			{
				MealPlanId = mealPlan.MealPlanId,
				Items = mealPlan.Recipes
					.SelectMany(r => r.Recipe.Ingredients.Split('\n')) // Assuming ingredients are newline-separated
					.GroupBy(ingredient => ingredient.Trim())
					.Select(group => new ShoppingListItem
					{
						Ingredient = group.Key
					}).ToList()
			};

			_dbContext.ShoppingLists.Add(shoppingList);
			await _dbContext.SaveChangesAsync();

			var response = new ShoppingListResponseDto
			{
				ShoppingListId = shoppingList.ShoppingListId,
				MealPlanId = shoppingList.MealPlanId,
				Items = shoppingList.Items.Select(i => new ShoppingListItemDto
				{
					ShoppingListItemId = i.ShoppingListItemId,
					Ingredient = i.Ingredient,
					IsPurchased = i.IsPurchased
				}).ToList()
			};

			return Ok(response);
		}

		// View shopping list for a meal plan
		[HttpGet("{mealPlanId}")]
		[Authorize(Roles = "User,MealPlanner,Admin")]
		public async Task<IActionResult> GetShoppingList(int mealPlanId)
		{
			var shoppingList = await _dbContext.ShoppingLists
				.Include(sl => sl.Items)
				.FirstOrDefaultAsync(sl => sl.MealPlanId == mealPlanId);

			if (shoppingList == null)
				return NotFound("Shopping list not found.");

			// Ownership check
			var mealPlan = await _dbContext.MealPlans.FindAsync(mealPlanId);
			if (mealPlan?.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to view this shopping list.");

			var response = new ShoppingListResponseDto
			{
				ShoppingListId = shoppingList.ShoppingListId,
				MealPlanId = shoppingList.MealPlanId,
				Items = shoppingList.Items.Select(i => new ShoppingListItemDto
				{
					ShoppingListItemId = i.ShoppingListItemId,
					Ingredient = i.Ingredient,
					IsPurchased = i.IsPurchased
				}).ToList()
			};

			return Ok(response);
		}

		// Mark shopping list items as purchased
		[HttpPut("mark-as-purchased")]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> MarkAsPurchased([FromBody] MarkPurchasedDto dto)
		{
			var shoppingList = await _dbContext.ShoppingLists
				.Include(sl => sl.Items)
				.FirstOrDefaultAsync(sl => sl.ShoppingListId == dto.ShoppingListId);

			if (shoppingList == null)
				return NotFound("Shopping list not found.");

			// Ownership check
			var mealPlan = await _dbContext.MealPlans.FindAsync(shoppingList.MealPlanId);
			if (mealPlan?.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to modify this shopping list.");

			foreach (var item in shoppingList.Items.Where(i => dto.ItemIds.Contains(i.ShoppingListItemId)))
			{
				item.IsPurchased = true;
			}

			_dbContext.ShoppingLists.Update(shoppingList);
			await _dbContext.SaveChangesAsync();

			return Ok("Items marked as purchased.");
		}
	}
}
