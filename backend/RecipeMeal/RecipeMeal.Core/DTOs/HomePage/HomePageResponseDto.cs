namespace RecipeMeal.Core.DTOs.HomePage
{
	public class HomePageResponseDto
	{
		public int MealPlanId { get; set; }
		public string MealPlanName { get; set; }
		public List<DailyMealDto> DailyMeals { get; set; }
		public DailyNutritionSummaryDto DailyNutrition { get; set; }
	}

	public class DailyMealDto
	{
		public int RecipeId { get; set; }
		public string RecipeTitle { get; set; }
		public string ImageUrl { get; set; }
		public DateTime MealTime { get; set; }
		public RecipeDetailDto RecipeDetails { get; set; }
	}

	public class RecipeDetailDto
	{
		public string Description { get; set; }
		public string Ingredients { get; set; }
		public string Steps { get; set; }
		public NutritionInfoDto Nutrition { get; set; }
	}

	public class NutritionInfoDto
	{
		public int Calories { get; set; }
		public float Protein { get; set; }
		public float Carbs { get; set; }
		public float Fat { get; set; }
		public string Vitamins { get; set; }
	}

	public class DailyNutritionSummaryDto
	{
		public int TotalCalories { get; set; }
		public float TotalProtein { get; set; }
		public float TotalCarbs { get; set; }
		public float TotalFat { get; set; }
	}
}
