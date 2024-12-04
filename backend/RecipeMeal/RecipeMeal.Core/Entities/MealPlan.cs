using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeMeal.Core.Entities
{
    public class MealPlan
    {
        [Key]
        public int MealPlanId { get; set; }

        [Required]
        public string Name { get; set; } // e.g., "Weekly Plan" or "Sunday Meal Plan"

        [Required]
        public string CreatedBy { get; set; } // Username of the user who created this

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public List<MealPlanRecipe> Recipes { get; set; } = new List<MealPlanRecipe>(); // Many-to-Many with Recipes
    }

    // Junction table for MealPlan and Recipes
    public class MealPlanRecipe
    {
        [Key]
        public int MealPlanRecipeId { get; set; }

        [ForeignKey("MealPlan")]
        public int MealPlanId { get; set; }
        public MealPlan MealPlan { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public DateTime MealTime { get; set; } // The time this recipe is planned for
    }
}
