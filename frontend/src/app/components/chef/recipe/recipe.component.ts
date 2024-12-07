import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RecipeService } from '../../../services/recipe.service';
import { ReviewService } from '../../../services/review.service';

@Component({
	selector: 'app-recipe',
	templateUrl: './recipe.component.html',
	standalone: true,
	imports: [CommonModule, FormsModule],
})
export class RecipeComponent  implements OnInit  {
	recipes: any[] = [];
	sortedRecipes: any[] = [];
	selectedRecipe: any = null;
	isEditMode: boolean = false;
	userRole: string | null = null;

	reviews: any[] = [];
	newReview = { rating: 0, comment: '' };
	mealTimeFilters: string[] = ['All', 'Snack', 'Lunch', 'Drink', 'Dinner', 'Breakfast', 'Dessert'];
  selectedMealTime: string = 'All'; // Default filter (show all)
  searchQuery: string = ''; // Search query for recipe name

	sortOption: string = 'ratingDesc';

	selectedImage: File | null = null;

	currentUsername: string = 'currentUser';
	ngOnInit(): void {
		// Retrieve the role from localStorage
		this.userRole = localStorage.getItem('userRole');
		console.log('User Role:', this.userRole);
	  }
	constructor(
		private recipeService: RecipeService,
		private reviewService: ReviewService
	) {
		this.loadRecipes();
	}

	loadRecipes() {
		this.recipeService.getAllRecipes().subscribe({
		  next: (data) => {
			const loadedRecipes = data?.$values || [];
			let pendingRatings = loadedRecipes.map((recipe: any) =>
			  this.reviewService.getAverageRating(recipe.recipeId).toPromise().then(avg => {
				recipe.averageRating = avg?.averageRating ?? 0;
			  })
			);

			Promise.all(pendingRatings).then(() => {
			  this.recipes = loadedRecipes;
			  this.applyFilters();
			});
		  },
		  error: (err) => alert(`Error loading recipes: ${err.error}`),
		});
	  }

	  applyFilters() {
		let filteredRecipes = [...this.recipes];

		// Filter by meal time if not "All"
		if (this.selectedMealTime !== 'All') {
		  filteredRecipes = filteredRecipes.filter(recipe => recipe.category === this.selectedMealTime);
		}

		// Filter by search query if not empty
		if (this.searchQuery.trim() !== '') {
		  const lowerCaseQuery = this.searchQuery.toLowerCase();
		  filteredRecipes = filteredRecipes.filter(recipe =>
			recipe.title.toLowerCase().includes(lowerCaseQuery)
		  );
		}

		// Apply sorting
		this.sortedRecipes = this.sortRecipes(filteredRecipes);
	  }

	  sortRecipes(recipes: any[]) {
		if (this.sortOption === 'ratingDesc') {
		  return recipes.sort((a, b) => b.averageRating - a.averageRating);
		} else if (this.sortOption === 'ratingAsc') {
		  return recipes.sort((a, b) => a.averageRating - b.averageRating);
		} else if (this.sortOption === 'createdAtDesc') {
		  return recipes.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
		} else if (this.sortOption === 'createdAtAsc') {
		  return recipes.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());
		}
		return recipes;
	  }

	  applySorting() {
		this.applyFilters(); // Reapply filters and sort after changing the sorting option
	  }

	  applyMealTimeFilter(filter: string) {
		this.selectedMealTime = filter;
		this.applyFilters();
	  }


	openAddDialog() {
		this.isEditMode = true;
		this.selectedRecipe = {
			title: '',
			description: '',
			ingredients: '',
			steps: '',
			category: '',
			imageUrl: null,
			averageRating: 0
		};
		this.selectedImage = null;
	}

	openViewDialog(recipe: any) {
		this.isEditMode = false;
		this.selectedRecipe = { ...recipe };
		this.loadReviews(recipe.recipeId);
		this.reviewService.getAverageRating(recipe.recipeId).subscribe({
			next: (res) => {
				this.selectedRecipe.averageRating = res?.averageRating ?? 0;
			},
			error: () => { }
		});
	}

	openEditDialog(recipe: any) {
		this.isEditMode = true;
		this.selectedRecipe = { ...recipe };
		this.selectedImage = null;
	}

	cancelEdit() {
		this.selectedRecipe = null;
		this.selectedImage = null;
		this.isEditMode = false;
	}

	loadReviews(recipeId: number) {
		this.reviewService.getReviewsByRecipeId(recipeId).subscribe({
			next: (data) => {
				this.reviews = data?.$values || [];
			},
			error: (err) => alert(`Error loading reviews: ${err.error}`),
		});
	}

	addReview() {
		if (!this.newReview.rating) {
			alert('Please provide a rating.');
			return;
		}

		const reviewDto = {
			recipeId: this.selectedRecipe.recipeId,
			rating: this.newReview.rating,
			comment: this.newReview.comment
		};

		this.reviewService.addReview(reviewDto).subscribe({
			next: () => {
				alert('Review added successfully.');
				this.newReview = { rating: 0, comment: '' };
				this.loadReviews(this.selectedRecipe.recipeId);
				this.reviewService.getAverageRating(this.selectedRecipe.recipeId).subscribe(avgRes => {
					this.selectedRecipe.averageRating = avgRes?.averageRating ?? 0;
				});
			},
			error: (err) => alert(`Error adding review: ${err.error}`),
		});
	}

	editReview(review: any) {
		const newComment = prompt("Edit your comment:", review.comment);
		if (newComment === null) return;

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

	onFileSelected(event: any) {
		const file: File = event.target.files[0];
		if (file) {
			this.selectedImage = file;
		}
	}

	saveChanges() {
		if (this.isEditMode) {
			if (this.selectedRecipe.recipeId) {
				this.updateRecipe();
			} else {
				this.addRecipe();
			}
		}
	}

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

	deleteRecipe(id: number) {
		if (confirm('Are you sure you want to delete this recipe?')) {
			this.recipeService.deleteRecipe(id).subscribe({
				next: () => {
					alert('Recipe deleted successfully.');
					this.loadRecipes();
					if (this.selectedRecipe && this.selectedRecipe.recipeId === id) {
						this.cancelEdit();
					}
				},
				error: (err) => alert(`Error deleting recipe: ${err.error}`),
			});
		}
	}
}
