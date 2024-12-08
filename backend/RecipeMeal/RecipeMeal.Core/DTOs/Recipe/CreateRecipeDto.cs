using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RecipeMeal.Core.DTOs.Recipe
{
    public class CreateRecipeDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Steps { get; set; }

        public string Category { get; set; }

        [Required]
        public IFormFile Image { get; set; } 
    }
}
