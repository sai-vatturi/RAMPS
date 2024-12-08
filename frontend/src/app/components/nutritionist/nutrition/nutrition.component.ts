import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NutritionService } from '../../../services/nutrition.service';
import { RecipeService } from '../../../services/recipe.service';

@Component({
	selector: 'app-nutrition',
	standalone: true,
	templateUrl: './nutrition.component.html',
	imports: [NgIf, NgFor, CommonModule, FormsModule]
})
export class NutritionComponent implements OnInit {
	pendingMeals: any[] = [];
	existingNutrition: any[] = [];
	selectedRecipeDetails: any = null;
	filteredNutrition: any[] = [];
	selectedNutritionDetails: any = null;
	nutritionData: any = {};
	isEditMode: boolean = false;
	selectedMeal: any = null;
	selectedSortOption: string = 'caloriesAsc';
	userRole: string | null = null;

	showEditModal: boolean = false;
	showDeleteModal: boolean = false;
	confirmNutritionId: number | null = null;

	showSuccessAlert: boolean = false;
	successMessage: string = '';

	showErrorAlert: boolean = false;
	errorMessage: string = '';

	nutritionFilters: string[] = ['All', 'Low Calories', 'High Protein', 'Low Carbs', 'Low Fat'];
	selectedNutritionFilter: string = 'All';
	nutritionSearchQuery: string = '';

	constructor(private nutritionService: NutritionService, private recipeService: RecipeService) {}

	ngOnInit() {
		this.userRole = localStorage.getItem('userRole');
		if (this.userRole !== 'User' && this.userRole !== 'MealPlanner') {
			this.loadPendingMeals();
		}
		this.loadAllNutrition();
	}

	loadPendingMeals() {
		this.nutritionService.getPendingMeals().subscribe({
			next: data => {
				this.pendingMeals = Array.isArray(data) ? data : data.$values || [];
			},
			error: err => this.showError('Error loading pending meals.')
		});
	}

	loadAllNutrition() {
		this.nutritionService.getAllNutrition().subscribe({
			next: data => {
				this.existingNutrition = Array.isArray(data) ? data : data.$values || [];
				this.applyNutritionFilters();
			},
			error: err => this.showError('Error loading existing nutrition.')
		});
	}

	applyNutritionFilters() {
		let filtered = [...this.existingNutrition];

		if (this.nutritionSearchQuery.trim() !== '') {
			const query = this.nutritionSearchQuery.toLowerCase();
			filtered = filtered.filter(nutrition => nutrition.recipeTitle.toLowerCase().includes(query));
		}

		if (this.selectedNutritionFilter !== 'All') {
			filtered = filtered.filter(nutrition => {
				switch (this.selectedNutritionFilter) {
					case 'Low Calories':
						return nutrition.calories < 300;
					case 'High Protein':
						return nutrition.protein > 20;
					case 'Low Carbs':
						return nutrition.carbs < 20;
					case 'Low Fat':
						return nutrition.fat < 10;
					default:
						return true;
				}
			});
		}
		filtered = this.sortNutrition(filtered);
		this.filteredNutrition = filtered;
	}

	sortNutrition(nutritionData: any[]): any[] {
		switch (this.selectedSortOption) {
			case 'caloriesAsc':
				return nutritionData.sort((a, b) => a.calories - b.calories);
			case 'caloriesDesc':
				return nutritionData.sort((a, b) => b.calories - a.calories);
			case 'proteinAsc':
				return nutritionData.sort((a, b) => a.protein - b.protein);
			case 'proteinDesc':
				return nutritionData.sort((a, b) => b.protein - a.protein);
			case 'carbsAsc':
				return nutritionData.sort((a, b) => a.carbs - b.carbs);
			case 'carbsDesc':
				return nutritionData.sort((a, b) => b.carbs - a.carbs);
			case 'fatAsc':
				return nutritionData.sort((a, b) => a.fat - b.fat);
			case 'fatDesc':
				return nutritionData.sort((a, b) => b.fat - a.fat);
			default:
				return nutritionData;
		}
	}
	applyNutritionFilter(filter: string) {
		this.selectedNutritionFilter = filter;
		this.applyNutritionFilters();
	}

	selectRecipe(recipeId: number) {
		this.fetchRecipeDetails(recipeId);
		this.fetchNutritionDetails(recipeId);
		this.nutritionData.recipeId = recipeId;
	}

	fetchRecipeDetails(recipeId: number) {
		this.recipeService.getRecipeById(recipeId).subscribe({
			next: data => {
				this.selectedRecipeDetails = data;
			},
			error: err => this.showError('Error fetching recipe details.')
		});
	}

	fetchNutritionDetails(recipeId: number) {
		this.nutritionService.getNutritionDetailsByRecipe(recipeId).subscribe({
			next: data => {
				this.selectedNutritionDetails = data;
			},
			error: err => {
				this.selectedNutritionDetails = null;
			}
		});
	}

	addNutrition() {
		if (!this.nutritionData.recipeId) {
			this.showError('Recipe ID is required!');
			return;
		}

		this.nutritionService.addNutrition(this.nutritionData).subscribe({
			next: () => {
				this.showSuccess('Nutrition added successfully!');
				this.nutritionData = {};
				this.loadPendingMeals();
				this.loadAllNutrition();
			},
			error: err => this.showError('Error adding nutrition.')
		});
	}

	openEditModal(nutrition: any) {
		this.isEditMode = true;
		this.selectedMeal = nutrition;
		this.nutritionData = { ...nutrition };
		this.showEditModal = true;
	}

	closeEditModal() {
		this.showEditModal = false;
		this.isEditMode = false;
		this.nutritionData = {};
		this.selectedMeal = null;
	}

	updateNutrition() {
		if (!this.selectedMeal?.nutritionId) {
			this.showError('No nutrition selected for editing!');
			return;
		}

		this.nutritionService.updateNutrition(this.selectedMeal.nutritionId, this.nutritionData).subscribe({
			next: () => {
				this.showSuccess('Nutrition updated successfully!');
				this.closeEditModal();
				this.loadAllNutrition();
			},
			error: err => this.showError('Error updating nutrition.')
		});
	}

	confirmDelete(nutritionId: number) {
		this.confirmNutritionId = nutritionId;
		this.showDeleteModal = true;
	}

	deleteNutrition(nutritionId: number) {
		if (nutritionId === null) return;

		this.nutritionService.deleteNutrition(nutritionId).subscribe({
			next: () => {
				this.showSuccess('Nutrition deleted successfully!');
				this.loadAllNutrition();
				this.closeDeleteModal();
			},
			error: err => this.showError('Error deleting nutrition.')
		});
	}

	closeDeleteModal() {
		this.showDeleteModal = false;
		this.confirmNutritionId = null;
	}

	resetForm() {
		this.nutritionData = {};
	}

	showSuccess(message: string) {
		this.successMessage = message;
		this.showSuccessAlert = true;
		setTimeout(() => {
			this.showSuccessAlert = false;
			this.successMessage = '';
		}, 3000);
	}

	showError(message: string) {
		this.errorMessage = message;
		this.showErrorAlert = true;
		setTimeout(() => {
			this.showErrorAlert = false;
			this.errorMessage = '';
		}, 3000);
	}
}
