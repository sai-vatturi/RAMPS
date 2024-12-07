import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RecipeService } from '../../../services/recipe.service';
import { ReviewService } from '../../../services/review.service';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class RecipeComponent {
  recipes: any[] = []; // List of all recipes
  selectedRecipe: any = null; // Currently selected recipe (for viewing/editing)
  isEditMode: boolean = false; // Toggle between view and edit modes

  reviews: any[] = []; // List of reviews for the selected recipe
  newReview = { rating: 0, comment: '' }; // Holds new review data

  selectedImage: File | null = null; // Holds selected image for upload (if applicable)

  // Assume we have a current logged-in username (replace with actual logic)
  currentUsername: string = 'currentUser';

  constructor(
    private recipeService: RecipeService,
    private reviewService: ReviewService
  ) {
    this.loadRecipes(); // Load recipes on component initialization
  }

  /**
   * Fetches all recipes from the backend, then fetches their average ratings.
   */
  loadRecipes() {
    this.recipeService.getAllRecipes().subscribe({
      next: (data) => {
        const loadedRecipes = data?.$values || [];
        // For each recipe, fetch average rating
        let pendingRatings = loadedRecipes.map((recipe: any) =>
          this.reviewService.getAverageRating(recipe.recipeId).toPromise().then(avg => {
            recipe.averageRating = avg?.averageRating ?? 0;
          })
        );

        Promise.all(pendingRatings).then(() => {
          this.recipes = loadedRecipes;
        });
      },
      error: (err) => alert(`Error loading recipes: ${err.error}`),
    });
  }

  /**
   * Opens the dialog to add a new recipe.
   */
  openAddDialog() {
    this.isEditMode = true; // Enable form mode
    this.selectedRecipe = {
      title: '',
      description: '',
      ingredients: '',
      steps: '',
      category: '',
      imageUrl: null, // Default value for new recipe
      averageRating: 0
    };
    this.selectedImage = null; // Reset file input
  }

  /**
   * Opens the dialog to view a recipe, loads reviews, and fetches average rating.
   * @param recipe The recipe to view
   */
  openViewDialog(recipe: any) {
    this.isEditMode = false; // Disable form mode
    // Deep copy recipe to avoid mutation
    this.selectedRecipe = { ...recipe };
    this.loadReviews(recipe.recipeId);
    // Refresh rating in case it changed
    this.reviewService.getAverageRating(recipe.recipeId).subscribe({
      next: (res) => {
        this.selectedRecipe.averageRating = res?.averageRating ?? 0;
      },
      error: () => { /* handle error if needed */ }
    });
  }

  /**
   * Opens the dialog in Edit Mode for the currently selected recipe.
   * @param recipe The recipe to edit.
   */
  openEditDialog(recipe: any) {
    this.isEditMode = true;
    this.selectedRecipe = { ...recipe };
    this.selectedImage = null; // Reset any previously selected image
  }

  /**
   * Cancels the current action (add/edit) and closes the dialog.
   */
  cancelEdit() {
    this.selectedRecipe = null; // Clear selected recipe
    this.selectedImage = null; // Reset file input
    this.isEditMode = false; // Exit form mode
  }

  /**
   * Loads reviews for a specific recipe from the backend.
   * @param recipeId The ID of the recipe for which reviews are loaded
   */
  loadReviews(recipeId: number) {
    this.reviewService.getReviewsByRecipeId(recipeId).subscribe({
      next: (data) => {
        this.reviews = data?.$values || []; // Populate reviews array
      },
      error: (err) => alert(`Error loading reviews: ${err.error}`),
    });
  }

  /**
   * Handles the addition of a new review for the selected recipe.
   */
  addReview() {
    if (!this.newReview.rating) {
      alert('Please provide a rating.');
      return;
    }

    const reviewDto = {
      recipeId: this.selectedRecipe.recipeId, // Recipe ID from selected recipe
      rating: this.newReview.rating,
      comment: this.newReview.comment
    };

    this.reviewService.addReview(reviewDto).subscribe({
      next: () => {
        alert('Review added successfully.');
        this.newReview = { rating: 0, comment: '' }; // Reset review form
        this.loadReviews(this.selectedRecipe.recipeId); // Reload reviews after adding
        // Update average rating
        this.reviewService.getAverageRating(this.selectedRecipe.recipeId).subscribe(avgRes => {
          this.selectedRecipe.averageRating = avgRes?.averageRating ?? 0;
        });
      },
      error: (err) => alert(`Error adding review: ${err.error}`),
    });
  }

  /**
   * Edit a review (frontend logic)
   */
  editReview(review: any) {
    const newComment = prompt("Edit your comment:", review.comment);
    if (newComment === null) return; // User canceled

    const newRating = Number(prompt("Edit your rating (1-5):", review.rating));
    if (!newRating || newRating < 1 || newRating > 5) {
      alert("Invalid rating.");
      return;
    }

    const updatedReview = { ...review, rating: newRating, comment: newComment };
    this.reviewService.updateReview(review.reviewId, updatedReview).subscribe({
      next: () => {
        alert('Review updated successfully.');
        this.loadReviews(this.selectedRecipe.recipeId);
        this.reviewService.getAverageRating(this.selectedRecipe.recipeId).subscribe(avgRes => {
          this.selectedRecipe.averageRating = avgRes?.averageRating ?? 0;
        });
      },
      error: (err) => alert(`Error updating review: ${err.error}`),
    });
  }

  /**
   * Delete a review after confirmation
   */
  deleteReview(review: any) {
    if (confirm('Are you sure you want to delete this review?')) {
      this.reviewService.deleteReview(review.reviewId).subscribe({
        next: () => {
          alert('Review deleted successfully.');
          this.loadReviews(this.selectedRecipe.recipeId);
          this.reviewService.getAverageRating(this.selectedRecipe.recipeId).subscribe(avgRes => {
            this.selectedRecipe.averageRating = avgRes?.averageRating ?? 0;
          });
        },
        error: (err) => alert(`Error deleting review: ${err.error}`),
      });
    }
  }

  /**
   * Handles the file selection event for image uploads.
   * @param event The file input change event
   */
  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedImage = file; // Store the selected file
    }
  }

  /**
   * Saves changes to a recipe, determining whether to add or update based on the presence of an ID.
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
        alert('Recipe added successfully.');
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
        alert('Recipe updated successfully.');
        this.loadRecipes();
        this.cancelEdit();
      },
      error: (err) => alert(`Error updating recipe: ${err.error}`),
    });
  }

  /**
   * Deletes a recipe after user confirmation.
   * @param id The ID of the recipe to delete
   */
  deleteRecipe(id: number) {
    if (confirm('Are you sure you want to delete this recipe?')) {
      this.recipeService.deleteRecipe(id).subscribe({
        next: () => {
          alert('Recipe deleted successfully.');
          this.loadRecipes();
          // If the deleted recipe was open in view, close it
          if (this.selectedRecipe && this.selectedRecipe.recipeId === id) {
            this.cancelEdit();
          }
        },
        error: (err) => alert(`Error deleting recipe: ${err.error}`),
      });
    }
  }
}
