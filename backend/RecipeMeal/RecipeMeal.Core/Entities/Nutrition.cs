using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class Nutrition
	{
		[Key]
		public int NutritionId { get; set; }

		[Required]
		public int RecipeId { get; set; }

		[Range(0, int.MaxValue)]
		public int Calories { get; set; }

		[Range(0, float.MaxValue)]
		public float Protein { get; set; }

		[Range(0, float.MaxValue)]
		public float Carbs { get; set; }

		[Range(0, float.MaxValue)]
		public float Fat { get; set; }

		[MaxLength(500)]
		public string? Vitamins { get; set; }

		// Navigation Property
		public Recipe Recipe { get; set; }
	}
}
