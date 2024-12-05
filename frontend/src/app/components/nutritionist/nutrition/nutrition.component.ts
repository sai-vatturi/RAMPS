import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NutritionService } from '../../../services/nutrition.service';

@Component({
  selector: 'app-nutrition',
  standalone: true,
  templateUrl: './nutrition.component.html',
  imports: [CommonModule, FormsModule],
})
export class NutritionComponent implements OnInit {
	pendingMeals: any[] = []; // Ensure it's initialized as an array
	existingNutrition: any[] = []; // Ensure it's initialized as an array
	nutritionData: any = {}; // For new nutrition or patch
	selectedMeal: any = null; // Selected meal for editing nutrition
	isEditMode: boolean = false;

	constructor(private nutritionService: NutritionService) {}

	ngOnInit() {
	  this.loadPendingMeals();
	  this.loadAllNutrition();
	}

	loadPendingMeals() {
	  this.nutritionService.getPendingMeals().subscribe({
		next: (data) => {
		  // Ensure the response is treated as an array
		  this.pendingMeals = Array.isArray(data) ? data : data.$values || [];
		},
		error: (err) => console.error('Error loading pending meals:', err),
	  });
	}

	loadAllNutrition() {
	  this.nutritionService.getAllNutrition().subscribe({
		next: (data) => {
		  // Ensure the response is treated as an array
		  this.existingNutrition = Array.isArray(data) ? data : data.$values || [];
		},
		error: (err) => console.error('Error loading existing nutrition:', err),
	  });
	}

	addNutrition() {
	  if (!this.nutritionData.recipeId) {
		alert('Recipe ID is required!');
		return;
	  }

	  this.nutritionService.addNutrition(this.nutritionData).subscribe({
		next: () => {
		  alert('Nutrition added successfully!');
		  this.nutritionData = {};
		  this.loadPendingMeals();
		  this.loadAllNutrition();
		},
		error: (err) => console.error('Error adding nutrition:', err),
	  });
	}

	editNutrition(nutrition: any) {
	  this.isEditMode = true;
	  this.selectedMeal = nutrition;
	  this.nutritionData = { ...nutrition };
	}

	updateNutrition() {
	  if (!this.selectedMeal?.nutritionId) {
		alert('No nutrition selected for editing!');
		return;
	  }

	  this.nutritionService.updateNutrition(this.selectedMeal.nutritionId, this.nutritionData).subscribe({
		next: () => {
		  alert('Nutrition updated successfully!');
		  this.isEditMode = false;
		  this.nutritionData = {};
		  this.loadAllNutrition();
		},
		error: (err) => console.error('Error updating nutrition:', err),
	  });
	}

	cancelEdit() {
	  this.isEditMode = false;
	  this.nutritionData = {};
	  this.selectedMeal = null;
	}
	deleteNutrition(nutritionId: number) {
		if (confirm('Are you sure you want to delete this nutrition entry?')) {
		  this.nutritionService.deleteNutrition(nutritionId).subscribe({
			next: () => {
			  alert('Nutrition deleted successfully!');
			  this.loadAllNutrition(); // Reload the list after deletion
			},
			error: (err) => console.error('Error deleting nutrition:', err),
		  });
		}
	  }

  }
