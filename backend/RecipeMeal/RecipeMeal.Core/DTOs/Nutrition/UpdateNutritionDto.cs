using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.DTOs.Nutrition
{
	public class UpdateNutritionDto
	{
		[Required]
		public int NutritionId { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Calories { get; set; }

		[Required]
		[Range(0, float.MaxValue)]
		public float Protein { get; set; }

		[Required]
		[Range(0, float.MaxValue)]
		public float Carbs { get; set; }

		[Required]
		[Range(0, float.MaxValue)]
		public float Fat { get; set; }

		[MaxLength(500)]
		public string? Vitamins { get; set; }
	}
}
