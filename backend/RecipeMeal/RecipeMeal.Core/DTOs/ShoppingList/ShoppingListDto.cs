namespace RecipeMeal.Core.DTOs.ShoppingList
{
	public class CreateUserShoppingListDto
	{
		public string Username { get; set; } // Add Username for list creation
		public int MealPlanId { get; set; }
	}

	public class UserShoppingListResponseDto
	{
		public int UserShoppingListId { get; set; }
		public string Username { get; set; }
		public int MealPlanId { get; set; }
		public List<UserShoppingListItemDto> Items { get; set; }
	}

	public class UserShoppingListItemDto
	{
		public int UserShoppingListItemId { get; set; }
		public string Ingredient { get; set; }
		public int Quantity { get; set; }
		public bool IsPurchased { get; set; }
	}


	public class MarkPurchasedDto
	{
		public int ShoppingListId { get; set; }
		public List<int> ItemIds { get; set; }
	}
}
