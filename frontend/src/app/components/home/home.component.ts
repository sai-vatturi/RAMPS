import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomeService } from '../../services/home.service';
import { RecipeService } from '../../services/recipe.service';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	standalone: true,
	imports: [CommonModule, FormsModule]
})
export class HomeComponent implements OnInit {
	selectedDate: string;
	mealPlan: any = null;
	isLoading: boolean = false;
	error: string | null = null;

	// Time periods for grouping meals
	timePeriods = {
		breakfast: { label: 'Breakfast', icon: 'fas fa-coffee' },
		morningSnack: { label: 'Morning Snack', icon: 'fas fa-apple-alt' },
		lunch: { label: 'Lunch', icon: 'fas fa-utensils' },
		eveningSnack: { label: 'Evening Snack', icon: 'fas fa-cookie' },
		dinner: { label: 'Dinner', icon: 'fas fa-moon' }
	};

	// Add new properties for recipe details dialog
	showRecipeModal: boolean = false;
	selectedRecipeDetails: any = null;
	isLoadingRecipeDetails: boolean = false;

	constructor(
		private homeService: HomeService,
		private recipeService: RecipeService // Add RecipeService
	) {
		// Format today's date as YYYY-MM-DD
		const today = new Date();
		this.selectedDate = today.toISOString().split('T')[0];
	}

	ngOnInit() {
		this.loadMealPlan();
	}

	loadMealPlan() {
		this.isLoading = true;
		this.error = null;

		this.homeService.getCurrentDayPlan(this.selectedDate).subscribe({
			next: data => {
				this.mealPlan = data;
				this.isLoading = false;
			},
			error: err => {
				if (err.status === 404) {
					this.error = 'No meal plan found for the selected date.';
				} else {
					this.error = 'Failed to load meal plan. Please try again later.';
				}
				this.mealPlan = null;
				this.isLoading = false;
			}
		});
	}

	onDateChange() {
		// Ensure the date is properly formatted before making the API call
		if (this.selectedDate) {
			this.loadMealPlan();
		}
	}

	getMealPeriod(mealTime: string): string {
		const hour = new Date(mealTime).getHours();
		if (hour >= 6 && hour < 10) return 'breakfast';
		if (hour >= 10 && hour < 12) return 'morningSnack';
		if (hour >= 12 && hour < 16) return 'lunch';
		if (hour >= 16 && hour < 19) return 'eveningSnack';
		return 'dinner';
	}

	groupMealsByPeriod() {
		const grouped: { [key: string]: any[] } = {
			breakfast: [],
			morningSnack: [],
			lunch: [],
			eveningSnack: [],
			dinner: []
		};

		if (this.mealPlan?.dailyMeals?.$values) {
			this.mealPlan.dailyMeals.$values.forEach((meal: any) => {
				const period = this.getMealPeriod(meal.mealTime);
				grouped[period].push(meal);
			});
		}

		return grouped;
	}

	getProgressBarColor(percentage: number): string {
		if (percentage < 30) return 'bg-green-500';
		if (percentage < 70) return 'bg-yellow-500';
		return 'bg-red-500';
	}

	calculateNutrientPercentage(nutrient: number, max: number): number {
		return Math.min((nutrient / max) * 100, 100);
	}

	// Add new methods for recipe details
	openRecipeDetails(recipeId: number) {
		this.isLoadingRecipeDetails = true;
		this.showRecipeModal = true;

		this.recipeService.getRecipeById(recipeId).subscribe({
			next: data => {
				this.selectedRecipeDetails = data;
				this.isLoadingRecipeDetails = false;
			},
			error: err => {
				console.error('Error loading recipe details:', err);
				this.isLoadingRecipeDetails = false;
			}
		});
	}

	closeRecipeModal() {
		this.showRecipeModal = false;
		this.selectedRecipeDetails = null;
	}

	get recipeSteps(): string[] {
		if (!this.selectedRecipeDetails?.steps) return [];
		return this.selectedRecipeDetails.steps.split(/\d+\./).filter((step: string) => step.trim());
	}
}
