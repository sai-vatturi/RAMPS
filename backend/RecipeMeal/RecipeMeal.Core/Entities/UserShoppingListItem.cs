using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class UserShoppingListItem
	{
		[Key]
		public int UserShoppingListItemId { get; set; }

		[Required]
		public int UserShoppingListId { get; set; }
		public UserShoppingList UserShoppingList { get; set; } // Foreign Key Relationship

		[Required]
		public string Ingredient { get; set; }

		[Range(1, int.MaxValue)]
		public int Quantity { get; set; } = 1;

		public bool IsPurchased { get; set; } = false;
	}


}
