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
  patchData: any = {};
  selectedImage: File | null = null;
  selectedRecipe: any = null;
  newRecipe: any = {
    title: '',
    description: '',
    ingredients: '',
    steps: '',
    category: '',
    image: null,
  };
  isEditMode: boolean = false;

  constructor(private recipeService: RecipeService) {
    this.loadRecipes();
  }

  loadRecipes() {
	this.recipeService.getAllRecipes().subscribe({
	  next: (data) => {
		// Extract the $values property from the response if present
		this.recipes = data?.$values || [];
	  },
	  error: (err) => alert(`Error: ${err.error}`),
	});
  }
  cancelEdit() {
    this.selectedRecipe = null;
    this.patchData = {};
    this.selectedImage = null;
  }

  onFileSelected(event: any) {
	this.selectedImage = event.target.files[0];
	if (this.isEditMode) {
	  this.patchData.image = this.selectedImage;
	} else {
	  this.newRecipe.image = this.selectedImage;
	}
  }

  addRecipe() {
    const formData = new FormData();
    formData.append('title', this.newRecipe.title);
    formData.append('description', this.newRecipe.description);
    formData.append('ingredients', this.newRecipe.ingredients);
    formData.append('steps', this.newRecipe.steps);
    formData.append('category', this.newRecipe.category);
    formData.append('image', this.newRecipe.image);

    this.recipeService.createRecipe(formData).subscribe({
      next: () => {
        alert('Recipe added successfully');
        this.loadRecipes();
        this.resetForm();
      },
      error: (err) => alert(`Error adding recipe: ${err.error}`),
    });
  }

  editRecipe(recipe: any) {
    this.isEditMode = true;
    this.selectedRecipe = recipe;
    this.newRecipe = { ...recipe };
  }

  updateRecipe() {
    const formData = new FormData();
    formData.append('title', this.newRecipe.title);
    formData.append('description', this.newRecipe.description);
    formData.append('ingredients', this.newRecipe.ingredients);
    formData.append('steps', this.newRecipe.steps);
    formData.append('category', this.newRecipe.category);
    if (this.newRecipe.image) {
      formData.append('image', this.newRecipe.image);
    }

    this.recipeService.updateRecipe(this.selectedRecipe.recipeId, formData).subscribe({
      next: () => {
        alert('Recipe updated successfully');
        this.loadRecipes();
        this.resetForm();
      },
      error: (err) => alert(`Error updating recipe: ${err.error}`),
    });
  }

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

  resetForm() {
    this.newRecipe = {
      title: '',
      description: '',
      ingredients: '',
      steps: '',
      category: '',
      image: null,
    };
    this.isEditMode = false;
    this.selectedRecipe = null;
  }
  patchRecipe() {
	const formData = new FormData();
	if (this.patchData.title) formData.append('title', this.patchData.title);
	if (this.patchData.description) formData.append('description', this.patchData.description);
	if (this.patchData.ingredients) formData.append('ingredients', this.patchData.ingredients);
	if (this.patchData.steps) formData.append('steps', this.patchData.steps);
	if (this.patchData.category) formData.append('category', this.patchData.category);
	if (this.selectedImage) formData.append('image', this.selectedImage);

	this.recipeService.patchRecipe(this.selectedRecipe.recipeId, formData).subscribe({
	  next: () => {
		alert('Recipe updated successfully.');
		this.cancelEdit();
		this.loadRecipes();
	  },
	  error: (err) => {
		const errorMessage = err.error?.message || 'An error occurred';
		alert(`Error updating recipe: ${errorMessage}`);
	  },
	});
  }
}
