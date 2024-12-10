// src/app/components/dietary-preferences/dietary-preferences.component.ts

import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NutritionService } from '../../services/nutrition.service';
import { RecipeService } from '../../services/recipe.service';

export interface Recipe {
	recipeId: number;
	title: string;
	description: string;
	ingredients: string;
	steps: string;
	category: string;
	imageUrl: string;
	createdBy: string;
	createdAt: string;
	updatedAt: string;
	nutrition?: Nutrition; // Make nutrition optional
}

export interface Nutrition {
	nutritionId: number;
	recipeId: number;
	recipeTitle: string;
	calories: number;
	protein: number;
	carbs: number;
	fat: number;
	vitamins: string;
}

@Component({
	selector: 'app-dietary-preferences',
	standalone: true,
	templateUrl: './dietary-preferences.component.html',
	styleUrls: ['./dietary-preferences.component.css'],
	imports: [CommonModule, FormsModule]
})
export class DietaryPreferencesComponent implements OnInit {
	// BMI Calculation Inputs
	weight: number | null = null;
	height: number | null = null;
	age: number | null = null;
	gender: string = 'Male';
	activityLevel: string = 'Sedentary';

	// BMI Result
	bmi: number | null = null;
	bmiCategory: string = '';

	// Nutrition Data
	allNutrition: any[] = [];
	goodToEatNutrition: any[] = [];
	badToEatNutrition: any[] = [];

	// UI Flags
	showBMRDialog: boolean = false;
	isLoading: boolean = false;

	// Alerts
	showSuccessAlert: boolean = false;
	successMessage: string = '';
	showErrorAlert: boolean = false;
	errorMessage: string = '';

	constructor(private nutritionService: NutritionService, private recipeService: RecipeService) {}

	ngOnInit(): void {}

	// Open BMI Dialog
	openBMRDialog() {
		this.showBMRDialog = true;
	}

	// Calculate BMI
	calculateBMR() {
		if (!this.weight || !this.height || !this.age) {
			this.showError('Please fill in all fields.');
			return;
		}

		// Calculate BMI
		const heightInMeters = this.height / 100;
		this.bmi = this.weight / (heightInMeters * heightInMeters);

		// Determine BMI Category
		if (this.bmi < 18.5) {
			this.bmiCategory = 'Underweight';
		} else if (this.bmi >= 18.5 && this.bmi < 24.9) {
			this.bmiCategory = 'Normal weight';
		} else if (this.bmi >= 25 && this.bmi < 29.9) {
			this.bmiCategory = 'Overweight';
		} else {
			this.bmiCategory = 'Obesity';
		}

		// Fetch and Categorize Nutrition Data
		this.fetchAndCategorizeNutrition();

		// Close the dialog
		this.showBMRDialog = false;
	}

	// Fetch All Nutrition Data
	fetchAndCategorizeNutrition() {
		this.isLoading = true;

		this.nutritionService.getAllNutrition().subscribe({
			next: nutritionData => {
				this.allNutrition = Array.isArray(nutritionData) ? nutritionData : nutritionData.$values || [];
				this.categorizeNutrition();
				this.isLoading = false;
			},
			error: () => {
				this.showError('Error fetching nutrition data.');
				this.isLoading = false;
			}
		});
	}

	// Categorize Nutrition Data
	categorizeNutrition() {
		this.goodToEatNutrition = [];
		this.badToEatNutrition = [];

		this.allNutrition.forEach(nutrition => {
			const { calories, protein, carbs, fat } = nutrition;

			switch (this.bmiCategory) {
				case 'Underweight':
					if (calories >= 300 && protein >= 15) {
						this.goodToEatNutrition.push(nutrition);
					} else {
						this.badToEatNutrition.push(nutrition);
					}
					break;
				case 'Normal weight':
					if (calories >= 200 && calories <= 400) {
						this.goodToEatNutrition.push(nutrition);
					} else {
						this.badToEatNutrition.push(nutrition);
					}
					break;
				case 'Overweight':
					if (calories < 400 && carbs < 30) {
						this.goodToEatNutrition.push(nutrition);
					} else {
						this.badToEatNutrition.push(nutrition);
					}
					break;
				case 'Obesity':
					if (calories < 300 && protein >= 20 && fat < 15) {
						this.goodToEatNutrition.push(nutrition);
					} else {
						this.badToEatNutrition.push(nutrition);
					}
					break;
				default:
					this.badToEatNutrition.push(nutrition);
			}
		});

		// Fetch Recipe Details for Categorized Nutrition
		this.fetchRecipeDetails(this.goodToEatNutrition);
		this.fetchRecipeDetails(this.badToEatNutrition);
	}

	// Fetch Recipe Details
	fetchRecipeDetails(nutritionList: any[]) {
		nutritionList.forEach(nutrition => {
			this.recipeService.getRecipeById(nutrition.recipeId).subscribe({
				next: recipe => {
					nutrition.recipeTitle = recipe.title;
					nutrition.imageUrl = recipe.imageUrl;
				},
				error: () => {
					this.showError(`Error fetching details for recipe ID: ${nutrition.recipeId}`);
				}
			});
		});
	}

	// Show Success Alert
	showSuccess(message: string) {
		this.successMessage = message;
		this.showSuccessAlert = true;
		setTimeout(() => {
			this.showSuccessAlert = false;
			this.successMessage = '';
		}, 3000);
	}

	// Show Error Alert
	showError(message: string) {
		this.errorMessage = message;
		this.showErrorAlert = true;
		setTimeout(() => {
			this.showErrorAlert = false;
			this.errorMessage = '';
		}, 3000);
	}

	// Reset BMI Calculation
	resetBMR() {
		this.bmi = null;
		this.bmiCategory = '';
		this.allNutrition = [];
		this.goodToEatNutrition = [];
		this.badToEatNutrition = [];
		this.showBMRDialog = false;
	}
}
