import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RecipeService } from '../../../services/recipe.service';

@Component({
  selector: 'app-recipe',
  standalone: true,
  templateUrl: './recipe.component.html',
  imports: [CommonModule, FormsModule],
})
export class RecipeComponent {
  recipes: any[] = [];
  selectedImage: File | null = null;
  selectedRecipe: any = null;
  isEditMode: boolean = false;

  constructor(private recipeService: RecipeService) {
    this.loadRecipes();
  }

  /**
   * Loads all recipes from the backend service.
   */
  loadRecipes() {
    this.recipeService.getAllRecipes().subscribe({
      next: (data) => {
        this.recipes = data?.$values || [];
      },
      error: (err) => alert(`Error: ${err.error}`),
    });
  }

  /**
   * Opens the dialog in Add Mode.
   * Initializes a new recipe object for binding.
   */
  openAddDialog() {
    this.isEditMode = true; // Enable form mode
    this.selectedRecipe = {
      title: '',
      description: '',
      ingredients: '',
      steps: '',
      category: '',
      image: null,
    };
    this.selectedImage = null; // Reset any previously selected image
  }

  /**
   * Opens the dialog in View Mode for a specific recipe.
   * @param recipe The recipe to view.
   */
  openViewDialog(recipe: any) {
    this.isEditMode = false; // Disable form mode
    // Create a shallow copy to prevent direct mutations
    this.selectedRecipe = { ...recipe };
  }

  /**
   * Opens the dialog in Edit Mode for a specific recipe.
   * @param recipe The recipe to edit.
   */
  openEditDialog(recipe: any) {
    this.isEditMode = true; // Enable form mode
    // Create a shallow copy to allow editing
    this.selectedRecipe = { ...recipe };
    this.selectedImage = null; // Reset any previously selected image
  }

  /**
   * Cancels the current action and closes the dialog.
   */
  cancelEdit() {
    this.selectedRecipe = null;
    this.selectedImage = null;
    this.isEditMode = false;
  }

  /**
   * Handles the file input change event to capture the selected image.
   * @param event The file input change event.
   */
  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedImage = file;
    }
  }

  /**
   * Adds a new recipe using the RecipeService.
   */
  addRecipe() {
    if (!this.selectedRecipe) return;

    const formData = new FormData();
    formData.append('title', this.selectedRecipe.title);
    formData.append('description', this.selectedRecipe.description);
    formData.append('ingredients', this.selectedRecipe.ingredients);
    formData.append('steps', this.selectedRecipe.steps);
    formData.append('category', this.selectedRecipe.category);
    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    this.recipeService.createRecipe(formData).subscribe({
      next: () => {
        alert('Recipe added successfully');
        this.loadRecipes();
        this.cancelEdit();
      },
      error: (err) => alert(`Error adding recipe: ${err.error}`),
    });
  }

  /**
   * Updates an existing recipe using the RecipeService.
   */
  updateRecipe() {
    if (!this.selectedRecipe || !this.selectedRecipe.recipeId) return;

    const formData = new FormData();
    formData.append('title', this.selectedRecipe.title);
    formData.append('description', this.selectedRecipe.description);
    formData.append('ingredients', this.selectedRecipe.ingredients);
    formData.append('steps', this.selectedRecipe.steps);
    formData.append('category', this.selectedRecipe.category);
    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    this.recipeService.updateRecipe(this.selectedRecipe.recipeId, formData).subscribe({
      next: () => {
        alert('Recipe updated successfully');
        this.loadRecipes();
        this.cancelEdit();
      },
      error: (err) => alert(`Error updating recipe: ${err.error}`),
    });
  }

  /**
   * Deletes a recipe after user confirmation.
   * @param id The ID of the recipe to delete.
   */
  deleteRecipe(id: number) {
    if (confirm('Are you sure you want to delete this recipe?')) {
      this.recipeService.deleteRecipe(id).subscribe({
        next: () => {
          alert('Recipe deleted successfully');
          this.loadRecipes();
        },
        error: (err) => alert(`Error deleting recipe: ${err.error}`),
      });
    }
  }

  /**
   * Determines whether to add or update a recipe based on the presence of recipeId.
   * Called upon form submission.
   */
  saveChanges() {
    if (this.isEditMode) {
      if (this.selectedRecipe.recipeId) {
        this.updateRecipe();
      } else {
        this.addRecipe();
      }
    }
  }
}
