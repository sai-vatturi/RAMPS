// meal-plan.interface.ts
export interface Recipe {
	recipeId: number;
	title: string;
	description: string;
	imageUrl: string;
	mealTime: string;
	preparationTime: number;
	cookingTime: number;
	servings: number;
	calories: number;
	ingredients: string[];
}

export interface MealPlan {
	mealPlanId: number;
	name: string;
	startDate: string;
	endDate: string;
	recipes: Recipe[];
	totalCalories?: number;
	totalMeals?: number;
}
