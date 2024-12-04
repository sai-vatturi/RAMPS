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
		public string Ingredients { get; set; } = string.Empty; // Stored as JSON or a delimited string

		[Required]
		public string Steps { get; set; } = string.Empty; // Stored as JSON or a delimited string

		[MaxLength(50)]
		public string Category { get; set; } = string.Empty; // E.g., Breakfast, Dinner, Vegan

		[Required]
		public string ImageUrl { get; set; } = string.Empty; // URL of the uploaded image

		[Required]
		public string CreatedBy { get; set; } = string.Empty; // User who created the recipe

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; }

		// One-to-Many Relationship with Reviews
		public ICollection<Review> Reviews { get; set; } = new List<Review>();

		// One-to-One Relationship with Nutrition
		public Nutrition Nutrition { get; set; }
	}
}
