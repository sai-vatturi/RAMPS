namespace RecipeMeal.Core.DTOs.Nutrition
{
	public class NutrientInfoDto
	{
		public int MealPlanId { get; set; }
		public double TotalCalories { get; set; }
		public double TotalProtein { get; set; }
		public double TotalCarbs { get; set; }
		public double TotalFat { get; set; }
		public double TotalVitamins { get; set; }
		public List<RecipeNutrientInfoDto> Recipes { get; set; }
	}

	public class RecipeNutrientInfoDto
	{
		public int RecipeId { get; set; }
		public string Title { get; set; }
		public double Calories { get; set; }
		public double Protein { get; set; }
		public double Carbs { get; set; }
		public double Fat { get; set; }
		public double Vitamins { get; set; }
	}
}
