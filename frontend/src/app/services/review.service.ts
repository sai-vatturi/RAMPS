import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class ReviewService {
	private baseUrl = 'http://localhost:5228/api/Review'; // Base URL for the Review API

	constructor(private http: HttpClient) {}

	/**
	 * Fetch all reviews for a specific recipe by its ID.
	 * @param recipeId The ID of the recipe for which reviews are being fetched.
	 * @returns Observable containing the list of reviews.
	 */
	getReviewsByRecipeId(recipeId: number): Observable<any> {
		return this.http.get(`${this.baseUrl}/recipe/${recipeId}`);
	}

	/**
	 * Fetch all reviews (restricted to Admin).
	 * @returns Observable containing the list of all reviews.
	 */
	getAllReviews(): Observable<any> {
		return this.http.get(this.baseUrl);
	}

	/**
	 * Fetch the average rating for a specific recipe by its ID.
	 * @param recipeId The ID of the recipe for which the average rating is being fetched.
	 * @returns Observable containing the average rating.
	 */
	getAverageRating(recipeId: number): Observable<any> {
		return this.http.get(`${this.baseUrl}/average-rating/${recipeId}`);
	}

	/**
	 * Add a new review for a specific recipe.
	 * @param review The review object containing recipeId, rating, and comment.
	 * @returns Observable containing the added review.
	 */
	addReview(review: any): Observable<any> {
		return this.http.post(this.baseUrl, review);
	}

	/**
	 * Update an existing review.
	 * @param reviewId The ID of the review to be updated.
	 * @param updatedReview The updated review object containing new rating and comment.
	 * @returns Observable containing the updated review.
	 */
	updateReview(reviewId: number, updatedReview: any): Observable<any> {
		return this.http.put(`${this.baseUrl}/${reviewId}`, updatedReview);
	}

	/**
	 * Delete a review by its ID.
	 * @param reviewId The ID of the review to be deleted.
	 * @returns Observable indicating the success or failure of the operation.
	 */
	deleteReview(reviewId: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/${reviewId}`);
	}
}
