namespace RecipeMeal.Core.DTOs.Review
{
	public class ReviewDto
	{
		public int RecipeId { get; set; } // Foreign key to Recipe
		public int Rating { get; set; } // Rating between 1 and 5
		public string Comment { get; set; } // Optional comment
	}
}
