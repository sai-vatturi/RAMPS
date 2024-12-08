import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class ReviewService {
	private baseUrl = 'http://localhost:5228/api/Review';

	constructor(private http: HttpClient) {}

	getReviewsByRecipeId(recipeId: number): Observable<any> {
		return this.http.get(`${this.baseUrl}/recipe/${recipeId}`);
	}

	getAllReviews(): Observable<any> {
		return this.http.get(this.baseUrl);
	}

	getAverageRating(recipeId: number): Observable<any> {
		return this.http.get(`${this.baseUrl}/average-rating/${recipeId}`);
	}

	addReview(review: any): Observable<any> {
		return this.http.post(this.baseUrl, review);
	}

	updateReview(reviewId: number, updatedReview: any): Observable<any> {
		return this.http.put(`${this.baseUrl}/${reviewId}`, updatedReview);
	}

	deleteReview(reviewId: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/${reviewId}`);
	}
}
