using System;
using System.Collections.Generic;

namespace RecipeMeal.Core.DTOs.MealPlan
{
	public class PatchMealPlanDto
	{
		public string Name { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public List<PatchMealPlanRecipeDto> Recipes { get; set; }
	}

	public class PatchMealPlanRecipeDto
	{
		public int? RecipeId { get; set; }
		public DateTime? MealTime { get; set; }
	}
}
