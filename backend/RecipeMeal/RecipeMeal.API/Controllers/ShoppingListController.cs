using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.Interfaces.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "User,Admin")]
	public class ShoppingController : ControllerBase
	{
		private readonly IShoppingService _shoppingService;

		public ShoppingController(IShoppingService shoppingService)
		{
			_shoppingService = shoppingService;
		}

		private string GetUsernameFromToken()
		{
			return User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
		}

		[HttpPost("generate")]
		public async Task<IActionResult> GenerateShoppingList(int mealPlanId)
		{
			try
			{
				var username = GetUsernameFromToken();
				int userId = await _shoppingService.GetUserIdFromTokenAsync(username);

				var shoppingList = await _shoppingService.GenerateShoppingListAsync(mealPlanId, userId);
				return Ok(shoppingList);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetShoppingLists()
		{
			var username = GetUsernameFromToken();
			int userId = await _shoppingService.GetUserIdFromTokenAsync(username);

			var shoppingLists = await _shoppingService.GetShoppingListsAsync(userId);
			return Ok(shoppingLists);
		}

		[HttpPut("mark-purchased/{itemId}")]
		public async Task<IActionResult> MarkAsPurchased(int itemId, bool isPurchased = true)
		{
			try
			{
				var username = GetUsernameFromToken();
				int userId = await _shoppingService.GetUserIdFromTokenAsync(username);

				var item = await _shoppingService.MarkAsPurchasedAsync(itemId, userId, isPurchased);
				return Ok(new { Message = isPurchased ? "Item marked as purchased." : "Item unmarked as purchased.", Item = item });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpDelete("delete-item/{itemId}")]
		public async Task<IActionResult> DeleteItem(int itemId)
		{
			try
			{
				var username = GetUsernameFromToken();
				int userId = await _shoppingService.GetUserIdFromTokenAsync(username);

				var item = await _shoppingService.DeleteItemAsync(itemId, userId);
				return Ok(new { Message = "Item deleted.", Item = item });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpDelete("delete-list/{listId}")]
		public async Task<IActionResult> DeleteShoppingList(int listId)
		{
			try
			{
				var username = GetUsernameFromToken();
				int userId = await _shoppingService.GetUserIdFromTokenAsync(username);

				var message = await _shoppingService.DeleteShoppingListAsync(listId, userId);
				return Ok(new { Message = message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}
	}
}
