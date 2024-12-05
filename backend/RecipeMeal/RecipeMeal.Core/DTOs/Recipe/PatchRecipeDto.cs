using Microsoft.AspNetCore.Http;

namespace RecipeMeal.Core.DTOs.Recipe
{
	public class PatchRecipeDto
	{
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Ingredients { get; set; }
		public string? Steps { get; set; }
		public string? Category { get; set; }
		public IFormFile? Image { get; set; } // Optional, if updating the image
	}
}
