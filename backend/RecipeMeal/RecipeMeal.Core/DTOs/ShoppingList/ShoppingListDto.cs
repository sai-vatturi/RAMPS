namespace RecipeMeal.Core.DTOs.ShoppingList
{
	public class CreateShoppingListDto
	{
		public int MealPlanId { get; set; }
	}

	public class ShoppingListResponseDto
	{
		public int ShoppingListId { get; set; }
		public int MealPlanId { get; set; }
		public List<ShoppingListItemDto> Items { get; set; }
	}

	public class ShoppingListItemDto
	{
		public int ShoppingListItemId { get; set; }
		public string Ingredient { get; set; }
		public bool IsPurchased { get; set; }
	}

	public class MarkPurchasedDto
	{
		public int ShoppingListId { get; set; }
		public List<int> ItemIds { get; set; }
	}
}
