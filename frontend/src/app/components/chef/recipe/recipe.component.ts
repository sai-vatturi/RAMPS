import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { jsPDF } from 'jspdf'; // Import jsPDF for PDF generation
import { RecipeService } from '../../../services/recipe.service';
import { ReviewService } from '../../../services/review.service';

@Component({
	selector: 'app-recipe',
	templateUrl: './recipe.component.html',
	standalone: true,
	imports: [CommonModule, FormsModule]
})
export class RecipeComponent implements OnInit {
	recipes: any[] = [];
	sortedRecipes: any[] = [];
	selectedRecipe: any = null;
	isEditMode: boolean = false;
	userRole: string | null = null;

	reviews: any[] = [];
	newReview = { rating: 0, comment: '' };
	mealTimeFilters: string[] = ['All', 'Snack', 'Lunch', 'Drink', 'Dinner', 'Breakfast', 'Dessert'];
	selectedMealTime: string = 'All';
	searchQuery: string = '';

	sortOption: string = 'ratingDesc';

	selectedImage: File | null = null;

	currentUsername: string = 'currentUser';

	// Add the loading flag
	isLoading: boolean = false;

	// Pagination and count-related properties
	pageNumber: number = 1;
	pageSize: number = 9;
	totalCount: number = 0;

	constructor(private recipeService: RecipeService, private reviewService: ReviewService) {}

	ngOnInit(): void {
		this.userRole = localStorage.getItem('userRole');
		console.log('User Role:', this.userRole);
		this.loadRecipes(); // Move loadRecipes here to ensure it's called after initialization
		this.loadRecipeCount(); // Load the recipe count on initialization
	}

	// Updated loadRecipes to support pagination
	loadRecipes() {
		this.isLoading = true; // Start loading
		this.recipeService.getAllRecipes(this.pageNumber, this.pageSize).subscribe({
			next: data => {
				const loadedRecipes = data?.$values || [];
				let pendingRatings = loadedRecipes.map((recipe: any) =>
					this.reviewService
						.getAverageRating(recipe.recipeId)
						.toPromise()
						.then(avg => {
							recipe.averageRating = avg?.averageRating ?? 0;
						})
				);

				Promise.all(pendingRatings)
					.then(() => {
						this.recipes = loadedRecipes;
						this.applyFilters();
						this.isLoading = false; // Loading finished
					})
					.catch(err => {
						console.error('Error processing ratings:', err);
						alert(`Error processing ratings: ${err.message}`);
						this.isLoading = false; // Ensure loading is stopped on error
					});
			},
			error: err => {
				alert(`Error loading recipes: ${err.error}`);
				this.isLoading = false; // Ensure loading is stopped on error
			}
		});
	}

	// New method to load the total count of recipes
	loadRecipeCount() {
		this.recipeService.getRecipeCount().subscribe({
			next: data => {
				this.totalCount = data.count || 0;
			},
			error: err => {
				console.error('Error loading recipe count:', err);
			}
		});
	}

	applyFilters() {
		let filteredRecipes = [...this.recipes];

		if (this.selectedMealTime !== 'All') {
			filteredRecipes = filteredRecipes.filter(recipe => recipe.category === this.selectedMealTime);
		}

		if (this.searchQuery.trim() !== '') {
			const lowerCaseQuery = this.searchQuery.toLowerCase();
			filteredRecipes = filteredRecipes.filter(recipe => recipe.title.toLowerCase().includes(lowerCaseQuery));
		}

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
		this.applyFilters();
	}

	get totalPages() {
		return Math.ceil(this.totalCount / this.pageSize);
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
			next: res => {
				this.selectedRecipe.averageRating = res?.averageRating ?? 0;
			},
			error: () => {}
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
			next: data => {
				this.reviews = data?.$values || [];
			},
			error: err => alert(`Error loading reviews: ${err.error}`)
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
			error: err => alert(`Error adding review: ${err.error}`)
		});
	}

	editReview(review: any) {
		const newComment = prompt('Edit your comment:', review.comment);
		if (newComment === null) return;

		const newRating = Number(prompt('Edit your rating (1-5):', review.rating));
		if (!newRating || newRating < 1 || newRating > 5) {
			alert('Invalid rating.');
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
			error: err => alert(`Error updating review: ${err.error}`)
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
				error: err => alert(`Error deleting review: ${err.error}`)
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
			error: err => alert(`Error adding recipe: ${err.error}`)
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
			error: err => alert(`Error updating recipe: ${err.error}`)
		});
	}

	deleteRecipe(id: number) {
		if (confirm('Are you sure you want to delete this recipe?')) {
			this.recipeService.deleteRecipe(id).subscribe({
				next: () => {
					alert('Recipe deleted successfully.');
					this.loadRecipes();
				},
				error: err => alert(`Error deleting recipe: ${err.error}`)
			});
		}
	}

	// Pagination Methods

	previousPage() {
		if (this.pageNumber > 1) {
			this.pageNumber--;
			this.loadRecipes();
		}
	}

	nextPage() {
		if (this.pageNumber * this.pageSize < this.totalCount) {
			this.pageNumber++;
			this.loadRecipes();
		}
	}
	async downloadRecipeAsPdf(recipe: any): Promise<void> {
		const doc = new jsPDF({
			orientation: 'portrait',
			unit: 'mm',
			format: 'a4'
		});

		const primaryColor = '#191B19';
		const accentColor = '#4CAF50';
		const secondaryColor = '#494949';

		// Header
		doc.setFillColor(accentColor);
		doc.rect(0, 0, 210, 40, 'F');

		// Header text
		doc.setTextColor('#FFFFFF');
		doc.setFontSize(26);
		doc.text(recipe.title, 15, 25);

		doc.setFillColor('#FFFFFF');
		doc.setTextColor(accentColor);
		doc.setFontSize(12);
		const categoryWidth = doc.getTextWidth(recipe.category) + 10;
		doc.roundedRect(15, 32, categoryWidth, 7, 1, 1, 'F');
		doc.text(recipe.category, 20, 37);

		let yPosition = 45;

		// Add circular image if available
		if (recipe.imageUrl) {
			try {
				const response = await fetch(recipe.imageUrl, { mode: 'cors' });
				const blob = await response.blob();
				const base64: string = await new Promise<string>((resolve, reject) => {
					const reader = new FileReader();
					reader.onloadend = () => resolve(reader.result as string);
					reader.onerror = reject;
					reader.readAsDataURL(blob);
				});

				// Create temporary image to get dimensions
				const img = new Image();
				await new Promise<void>((resolve, reject) => {
					img.onload = () => resolve();
					img.onerror = reject;
					img.src = base64;
				});

				// Calculate dimensions for circular image
				const circleSize = 40; // 40mm diameter
				const xCenter = doc.internal.pageSize.getWidth() / 2 - circleSize / 2;
				yPosition += 10; // Add some spacing after header

				// Calculate dimensions to maintain aspect ratio
				let imageWidth = circleSize;
				let imageHeight = circleSize;
				const aspectRatio = img.width / img.height;

				if (aspectRatio > 1) {
					// Image is wider than tall
					imageHeight = circleSize / aspectRatio;
					// Center vertically
					const yOffset = (circleSize - imageHeight) / 2;
					doc.addImage(base64, 'JPEG', xCenter, yPosition + yOffset, imageWidth, imageHeight);
				} else {
					// Image is taller than wide or square
					imageWidth = circleSize * aspectRatio;
					// Center horizontally
					const xOffset = (circleSize - imageWidth) / 2;
					doc.addImage(base64, 'JPEG', xCenter + xOffset, yPosition, imageWidth, imageHeight);
				}

				yPosition += circleSize + 20; // Add spacing after image
			} catch (error) {
				console.warn('Failed to load image:', error);
				yPosition += 15; // Add some spacing even if image fails
			}
		} else {
			yPosition += 15; // Add some spacing if no image
		}

		// Function to add recipe content (Description, Ingredients, Steps, Footer)
		const addRecipeContent = () => {
			// Description Section
			doc.setTextColor(accentColor);
			doc.setFontSize(16);
			doc.text('Description', 15, yPosition);

			doc.setTextColor(secondaryColor);
			doc.setFontSize(12);
			const splitDescription = doc.splitTextToSize(recipe.description, 180);
			yPosition += 10;
			doc.text(splitDescription, 15, yPosition);
			yPosition += splitDescription.length * 7 + 10;

			// Ingredients Section
			doc.setTextColor(accentColor);
			doc.setFontSize(16);
			doc.text('Ingredients', 15, yPosition);
			yPosition += 10;

			doc.setTextColor(secondaryColor);
			doc.setFontSize(12);
			const ingredients: string[] = recipe.ingredients.split(',');
			ingredients.forEach((ingredient: string) => {
				const bulletPoint = '•';
				doc.text(bulletPoint, 15, yPosition);
				const splitIngredient = doc.splitTextToSize(ingredient.trim(), 170);
				doc.text(splitIngredient, 25, yPosition);
				yPosition += splitIngredient.length * 7;
			});
			yPosition += 10;

			// Steps Section
			doc.setTextColor(accentColor);
			doc.setFontSize(16);
			doc.text('Steps', 15, yPosition);
			yPosition += 10;

			doc.setTextColor(secondaryColor);
			doc.setFontSize(12);
			const steps: string[] = recipe.steps
				.split(/\d+\.\s?/)
				.map((step: string) => step.replace(/\\n/g, '').trim())
				.filter((step: string) => step !== '');
			steps.forEach((step: string, index: number) => {
				const stepNumber = `${index + 1}.`;
				doc.text(stepNumber, 15, yPosition);
				const splitStep = doc.splitTextToSize(step, 170);
				doc.text(splitStep, 25, yPosition);
				yPosition += splitStep.length * 7;
			});

			// Simplified Footer
			const pageCount = (doc as any).internal.pages.length;
			doc.setTextColor('#888888');
			doc.setFontSize(10);
			for (let i = 1; i <= pageCount; i++) {
				doc.setPage(i);
				doc.text('Generated by FoodPro', doc.internal.pageSize.getWidth() / 2, doc.internal.pageSize.getHeight() - 10, { align: 'center' });
			}

			const sanitizedTitle = recipe.title.replace(/[^a-z0-9]/gi, '_').toLowerCase();
			doc.save(`${sanitizedTitle}_recipe.pdf`);
		};

		addRecipeContent();
	}
}
