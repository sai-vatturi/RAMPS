import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MealPlanService } from '../../../services/meal-plan.service';

@Component({
  selector: 'app-meal-plan',
  templateUrl: './meal-plan.component.html',
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class MealPlanComponent implements OnInit {
  mealPlans: any[] = [];
  availableRecipes: any[] = []; // To fetch and show available recipes
  isEditMode: boolean = false;
  isAddMode: boolean = false;
  userRole: string | null = null;

  currentMealPlan: any = {
    name: '',
    startDate: '',
    endDate: '',
    recipes: [],
  };

  ngOnInit() {
    this.userRole = localStorage.getItem('userRole'); // Get user role
    this.loadMealPlans();
    if (this.userRole === 'MealPlanner' || this.userRole === 'Admin') {
      this.loadAvailableRecipes();
    }
  }

  constructor(private mealPlanService: MealPlanService) {
    this.loadMealPlans();
    this.loadAvailableRecipes();
  }

  startAddMode() {
	this.isAddMode = true;
	this.resetForm();
  }


  loadMealPlans() {
	this.mealPlanService.getAllMealPlans().subscribe({
	  next: (data) => {
		this.mealPlans = (data.$values || data).map((plan: any) => ({
		  ...plan,
		  recipes: (plan.recipes?.$values || plan.recipes || []).map((recipe: any) => ({
			...recipe,
			mealPeriod: this.getMealPeriod(recipe.mealTime),
		  })),
		}));
	  },
	  error: (err) => alert(`Error loading meal plans: ${err.error}`),
	});
  }


  loadAvailableRecipes() {
    this.mealPlanService.getAvailableRecipes().subscribe({
      next: (data) => {
        this.availableRecipes = data.$values || data;
      },
      error: (err) => alert(`Error loading recipes: ${err.error}`),
    });
  }

  getMealPeriod(mealTime: string): string {
    const hour = new Date(mealTime).getHours();
    if (hour >= 6 && hour < 10) return 'Breakfast';
    if (hour >= 10 && hour < 12) return 'Morning Snack';
    if (hour >= 12 && hour < 16) return 'Lunch';
    if (hour >= 16 && hour < 18) return 'Afternoon Snack';
    if (hour >= 18 && hour <= 22) return 'Dinner';
    return 'Late Snack';
  }

  editMealPlan(plan: any) {
    this.isEditMode = true;
    this.currentMealPlan = {
      ...plan,
      recipes: plan.recipes.map((r: any) => ({ ...r })),
    };
  }

  createMealPlan() {
    this.mealPlanService.createMealPlan(this.currentMealPlan).subscribe({
      next: () => {
        alert('Meal Plan created successfully');
        this.loadMealPlans();
        this.resetForm();
      },
      error: (err) => alert(`Error creating meal plan: ${err.error}`),
    });
  }

  updateMealPlan() {
    this.mealPlanService.updateMealPlan(this.currentMealPlan.mealPlanId, this.currentMealPlan).subscribe({
      next: () => {
        alert('Meal Plan updated successfully');
        this.loadMealPlans();
        this.resetForm();
      },
      error: (err) => alert(`Error updating meal plan: ${err.error}`),
    });
  }

  deleteMealPlan(id: number) {
    if (confirm('Are you sure you want to delete this meal plan?')) {
      this.mealPlanService.deleteMealPlan(id).subscribe({
        next: () => {
          alert('Meal Plan deleted successfully');
          this.loadMealPlans();
        },
        error: (err) => alert(`Error deleting meal plan: ${err.error}`),
      });
    }
  }

  addRecipe() {
    this.currentMealPlan.recipes.push({ recipeId: '', mealTime: '' });
  }

  removeRecipe(index: number) {
    this.currentMealPlan.recipes.splice(index, 1);
  }

  cancelEdit() {
    this.isEditMode = false;
    this.isAddMode = false;
    this.resetForm();
  }

  resetForm() {
    this.currentMealPlan = {
      name: '',
      startDate: '',
      endDate: '',
      recipes: [],
    };
  }
}
