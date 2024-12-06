using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class ShoppingList
	{
		public int ShoppingListId { get; set; }
		public int MealPlanId { get; set; }
		public MealPlan MealPlan { get; set; } // Relationship
		public List<ShoppingListItem> Items { get; set; } = new();
	}

	public class ShoppingListItem
	{
		public int ShoppingListItemId { get; set; }
		public int ShoppingListId { get; set; }
		public ShoppingList ShoppingList { get; set; } // Relationship

		[Required]
		[MaxLength(100)]
		public string Ingredient { get; set; } // Correctly added ItemName

		[Range(1, int.MaxValue)]
		public int Quantity { get; set; } = 1; // Correctly added Quantity
	}
}
