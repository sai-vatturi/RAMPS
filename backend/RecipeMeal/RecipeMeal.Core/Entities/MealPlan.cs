using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeMeal.Core.Entities
{
	public class MealPlan
	{
		[Key]
		public int MealPlanId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string CreatedBy { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

		public List<MealPlanRecipe> Recipes { get; set; } = new List<MealPlanRecipe>();
	}

	public class MealPlanRecipe
	{
		[Key]
		public int MealPlanRecipeId { get; set; }

		[ForeignKey("MealPlan")]
		public int MealPlanId { get; set; }
		public MealPlan MealPlan { get; set; }

		[ForeignKey("Recipe")]
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; }

		public DateTime MealTime { get; set; }
	}
}
