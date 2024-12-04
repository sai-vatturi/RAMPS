using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class Review
	{
		public int ReviewId { get; set; }

		[Required]
		public int RecipeId { get; set; } // Foreign key to Recipe

		[Required]
		public string UserName { get; set; } = string.Empty; // Name of the user posting the review

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; } // Rating between 1 and 5

		[MaxLength(500)]
		public string Comment { get; set; } = string.Empty; // Optional comment

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation property
		public Recipe Recipe { get; set; }
	}
}
