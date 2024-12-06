namespace RecipeMeal.Core.DTOs.ShoppingList
{
	public class UpdateShoppingListItemDto
	{
		public int ShoppingListItemId { get; set; }
		public string Ingredient { get; set; }
		public int Quantity { get; set; }
	}

}
