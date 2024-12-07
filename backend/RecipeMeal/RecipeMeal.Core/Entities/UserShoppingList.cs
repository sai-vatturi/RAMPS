using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class UserShoppingList
	{
		[Key]
		public int UserShoppingListId { get; set; }

		[Required]
		public int UserId { get; set; } // Links to the user.

		[Required]
		public int MealPlanId { get; set; } // Links to the meal plan.

		public List<UserShoppingListItem> Items { get; set; } = new();
	}

}
