using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.Entities
{
	public class Recipe
	{
		[Key]
		public int RecipeId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; } = string.Empty;

		[Required]
		public string Description { get; set; } = string.Empty;

		[Required]
		public string Ingredients { get; set; } = string.Empty;

		[Required]
		public string Steps { get; set; } = string.Empty;

		[MaxLength(50)]
		public string Category { get; set; } = string.Empty;

		[Required]
		public string ImageUrl { get; set; } = string.Empty;

		[Required]
		public string CreatedBy { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; }

		public ICollection<Review> Reviews { get; set; } = new List<Review>();

		public Nutrition Nutrition { get; set; }
	}
}
