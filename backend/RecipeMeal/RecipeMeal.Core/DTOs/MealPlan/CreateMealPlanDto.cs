using System;
using System.Collections.Generic;

namespace RecipeMeal.Core.DTOs.MealPlan
{
    public class CreateMealPlanDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MealPlanRecipeDto> Recipes { get; set; } // Recipe details
    }

    public class MealPlanRecipeDto
    {
        public int RecipeId { get; set; }
        public DateTime MealTime { get; set; }
    }
}
