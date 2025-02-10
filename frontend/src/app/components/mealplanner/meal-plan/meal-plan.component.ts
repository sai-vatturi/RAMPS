import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MealPlanService } from '../../../services/meal-plan.service';
import { RecipeService } from '../../../services/recipe.service'; // Add this import

@Component({
	selector: 'app-meal-plan',
	templateUrl: './meal-plan.component.html',
	standalone: true,
	imports: [FormsModule, CommonModule]
})
export class MealPlanComponent implements OnInit {
	mealPlans: any[] = [];
	availableRecipes: any[] = [];
	isEditMode: boolean = false;
	isAddMode: boolean = false;
	userRole: string | null = null;

	currentMealPlan: any = {
		name: '',
		startDate: '',
		endDate: '',
		recipes: []
	};

	// Loading Flags
	isLoadingMealPlans: boolean = false;
	isLoadingAvailableRecipes: boolean = false;
	isLoadingCreateUpdate: boolean = false;

	constructor(
		private mealPlanService: MealPlanService,
		private recipeService: RecipeService // Inject RecipeService
	) {}

	ngOnInit() {
		this.userRole = localStorage.getItem('userRole');
		this.loadMealPlans();
		if (this.userRole === 'MealPlanner' || this.userRole === 'Admin') {
			this.loadAvailableRecipes();
		}
	}

	// Start Add Mode
	startAddMode() {
		this.isAddMode = true;
		this.resetForm();
	}

	// Load All Meal Plans
	loadMealPlans() {
		this.isLoadingMealPlans = true; // Start loading
		this.mealPlanService.getAllMealPlans().subscribe({
			next: data => {
				const mealPlans = data.$values || data;

				// Fetch recipe details for each meal plan
				const recipePromises = mealPlans.map((plan: any) => {
					const recipeDetailsPromises = (plan.recipes?.$values || plan.recipes || []).map((recipe: any) => {
						return this.recipeService
							.getRecipeById(recipe.recipeId)
							.toPromise()
							.then((recipeDetails: any) => {
								return {
									...recipe,
									title: recipeDetails.title,
									imageUrl: recipeDetails.imageUrl, // Fetch image URL from recipe details
									mealPeriod: this.getMealPeriod(recipe.mealTime)
								};
							})
							.catch(() => {
								return {
									...recipe,
									title: 'Unknown Recipe',
									imageUrl: 'https://via.placeholder.com/150?text=No+Image', // Fallback image
									mealPeriod: this.getMealPeriod(recipe.mealTime)
								};
							});
					});

					return Promise.all(recipeDetailsPromises).then(recipes => {
						return {
							...plan,
							recipes
						};
					});
				});

				Promise.all(recipePromises)
					.then(updatedMealPlans => {
						this.mealPlans = updatedMealPlans;
						this.isLoadingMealPlans = false; // Loading finished
					})
					.catch(() => {
						this.isLoadingMealPlans = false; // Ensure loading is stopped on error
					});
			},
			error: err => {
				alert(`Error loading meal plans: ${err.error}`);
				this.isLoadingMealPlans = false; // Ensure loading is stopped on error
			}
		});
	}

	// Load Available Recipes
	loadAvailableRecipes() {
		this.isLoadingAvailableRecipes = true; // Start loading
		this.mealPlanService.getAvailableRecipes().subscribe({
			next: data => {
				this.availableRecipes = data.$values || data;
				this.isLoadingAvailableRecipes = false; // Loading finished
			},
			error: err => {
				alert(`Error loading recipes: ${err.error}`);
				this.isLoadingAvailableRecipes = false; // Ensure loading is stopped on error
			}
		});
	}

	// Get Meal Period based on Time
	getMealPeriod(mealTime: string): string {
		const hour = new Date(mealTime).getHours();
		if (hour >= 6 && hour < 10) return 'Breakfast';
		if (hour >= 10 && hour < 12) return 'Morning Snack';
		if (hour >= 12 && hour < 16) return 'Lunch';
		if (hour >= 16 && hour < 18) return 'Afternoon Snack';
		if (hour >= 18 && hour <= 22) return 'Dinner';
		return 'Late Snack';
	}

	// Edit Meal Plan
	editMealPlan(plan: any) {
		this.isEditMode = true;
		this.isAddMode = false;
		this.currentMealPlan = {
			...plan,
			recipes: plan.recipes.map((r: any) => ({ ...r }))
		};
	}

	// Create Meal Plan
	createMealPlan() {
		this.isLoadingCreateUpdate = true; // Start loading
		this.mealPlanService.createMealPlan(this.currentMealPlan).subscribe({
			next: () => {
				alert('Meal Plan created successfully');
				this.loadMealPlans();
				this.resetForm();
				this.isAddMode = false;
				this.isLoadingCreateUpdate = false; // Loading finished
			},
			error: err => {
				alert(`Error creating meal plan: ${err.error}`);
				this.isLoadingCreateUpdate = false; // Ensure loading is stopped on error
			}
		});
	}

	// Update Meal Plan
	updateMealPlan() {
		this.isLoadingCreateUpdate = true; // Start loading
		this.mealPlanService.updateMealPlan(this.currentMealPlan.mealPlanId, this.currentMealPlan).subscribe({
			next: () => {
				alert('Meal Plan updated successfully');
				this.loadMealPlans();
				this.resetForm();
				this.isEditMode = false;
				this.isLoadingCreateUpdate = false; // Loading finished
			},
			error: err => {
				alert(`Error updating meal plan: ${err.error}`);
				this.isLoadingCreateUpdate = false; // Ensure loading is stopped on error
			}
		});
	}

	// Delete Meal Plan
	deleteMealPlan(id: number) {
		if (confirm('Are you sure you want to delete this meal plan?')) {
			this.mealPlanService.deleteMealPlan(id).subscribe({
				next: () => {
					alert('Meal Plan deleted successfully');
					this.loadMealPlans();
				},
				error: err => alert(`Error deleting meal plan: ${err.error}`)
			});
		}
	}

	// Add Recipe to Meal Plan
	addRecipe() {
		this.currentMealPlan.recipes.push({ recipeId: '', mealTime: '' });
	}

	// Remove Recipe from Meal Plan
	removeRecipe(index: number) {
		this.currentMealPlan.recipes.splice(index, 1);
	}

	// Cancel Edit/Add Mode
	cancelEdit() {
		this.isEditMode = false;
		this.isAddMode = false;
		this.resetForm();
	}

	// Reset Form
	resetForm() {
		this.currentMealPlan = {
			name: '',
			startDate: '',
			endDate: '',
			recipes: []
		};
	}
}
