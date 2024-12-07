import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NutritionService } from '../../../services/nutrition.service';
import { RecipeService } from '../../../services/recipe.service';

@Component({
  selector: 'app-nutrition',
  standalone: true,
  templateUrl: './nutrition.component.html',
  imports: [NgIf, NgFor, CommonModule, FormsModule],
})
export class NutritionComponent implements OnInit {
  pendingMeals: any[] = [];
  existingNutrition: any[] = [];
  selectedRecipeDetails: any = null;
  selectedNutritionDetails: any = null;
  nutritionData: any = {};
  isEditMode: boolean = false;
  selectedMeal: any = null;

  // Modal Controls
  showEditModal: boolean = false;
  showDeleteModal: boolean = false;
  confirmNutritionId: number | null = null;

  // Alert Controls
  showSuccessAlert: boolean = false;
  successMessage: string = '';

  showErrorAlert: boolean = false;
  errorMessage: string = '';

  constructor(
    private nutritionService: NutritionService,
    private recipeService: RecipeService
  ) {}

  ngOnInit() {
    this.loadPendingMeals();
    this.loadAllNutrition();
  }

  // Load Pending Meals
  loadPendingMeals() {
    this.nutritionService.getPendingMeals().subscribe({
      next: (data) => {
        this.pendingMeals = Array.isArray(data) ? data : data.$values || [];
      },
      error: (err) => this.showError('Error loading pending meals.'),
    });
  }

  // Load All Nutrition Entries
  loadAllNutrition() {
    this.nutritionService.getAllNutrition().subscribe({
      next: (data) => {
        this.existingNutrition = Array.isArray(data) ? data : data.$values || [];
      },
      error: (err) => this.showError('Error loading existing nutrition.'),
    });
  }

  // Select a Recipe to View Details
  selectRecipe(recipeId: number) {
    this.fetchRecipeDetails(recipeId);
    this.fetchNutritionDetails(recipeId);
    this.nutritionData.recipeId = recipeId; // Automatically set recipe ID for adding nutrition
  }

  // Fetch Recipe Details
  fetchRecipeDetails(recipeId: number) {
    this.recipeService.getRecipeById(recipeId).subscribe({
      next: (data) => {
        this.selectedRecipeDetails = data;
      },
      error: (err) => this.showError('Error fetching recipe details.'),
    });
  }

  // Fetch Nutrition Details
  fetchNutritionDetails(recipeId: number) {
    this.nutritionService.getNutritionDetailsByRecipe(recipeId).subscribe({
      next: (data) => {
        this.selectedNutritionDetails = data;
      },
      error: (err) => {
        this.selectedNutritionDetails = null; // Clear nutrition details if not found
      },
    });
  }

  // Add Nutrition Entry
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
      error: (err) => this.showError('Error adding nutrition.'),
    });
  }

  // Open Edit Modal
  openEditModal(nutrition: any) {
    this.isEditMode = true;
    this.selectedMeal = nutrition;
    this.nutritionData = { ...nutrition }; // Pre-fill the form with existing data
    this.showEditModal = true;
  }

  // Close Edit Modal
  closeEditModal() {
    this.showEditModal = false;
    this.isEditMode = false;
    this.nutritionData = {};
    this.selectedMeal = null;
  }

  // Update Nutrition Entry
  updateNutrition() {
    if (!this.selectedMeal?.nutritionId) {
      this.showError('No nutrition selected for editing!');
      return;
    }

    this.nutritionService
      .updateNutrition(this.selectedMeal.nutritionId, this.nutritionData)
      .subscribe({
        next: () => {
          this.showSuccess('Nutrition updated successfully!');
          this.closeEditModal();
          this.loadAllNutrition();
        },
        error: (err) => this.showError('Error updating nutrition.'),
      });
  }

  // Confirm Deletion
  confirmDelete(nutritionId: number) {
    this.confirmNutritionId = nutritionId;
    this.showDeleteModal = true;
  }

  // Delete Nutrition Entry
  deleteNutrition(nutritionId: number) {
    if (nutritionId === null) return;

    this.nutritionService.deleteNutrition(nutritionId).subscribe({
      next: () => {
        this.showSuccess('Nutrition deleted successfully!');
        this.loadAllNutrition();
        this.closeDeleteModal();
      },
      error: (err) => this.showError('Error deleting nutrition.'),
    });
  }

  // Close Delete Modal
  closeDeleteModal() {
    this.showDeleteModal = false;
    this.confirmNutritionId = null;
  }

  // Reset Add Nutrition Form
  resetForm() {
    this.nutritionData = {};
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
}
