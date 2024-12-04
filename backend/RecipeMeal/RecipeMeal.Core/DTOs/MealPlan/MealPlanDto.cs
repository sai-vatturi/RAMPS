namespace RecipeMeal.Core.DTOs.MealPlan
{

	public class MealPlanDto
	{
		public int MealPlanId { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<RecipeDto> Recipes { get; set; }
	}

	public class RecipeDto
	{
		public int RecipeId { get; set; }
		public string Title { get; set; }
		public DateTime MealTime { get; set; }
	}

}
