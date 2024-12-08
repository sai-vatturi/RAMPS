using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class Review
	{
		public int ReviewId { get; set; }

		[Required]
		public int RecipeId { get; set; }

		[Required]
		public string UserName { get; set; } = string.Empty;

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; }

		[MaxLength(500)]
		public string Comment { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public Recipe Recipe { get; set; }
	}
}
