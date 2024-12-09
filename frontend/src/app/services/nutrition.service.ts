import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class NutritionService {
	private baseUrl = 'http://15.207.100.163:8080/api/Nutrition';
	private nutritionDetailsBaseUrl = 'http://15.207.100.163:8080/api/NutritionDetails';

	constructor(private http: HttpClient) {}

	getPendingMeals(): Observable<any> {
		return this.http.get(`${this.baseUrl}/pending`);
	}

	getAllNutrition(): Observable<any> {
		return this.http.get(this.baseUrl);
	}

	addNutrition(data: any): Observable<any> {
		return this.http.post(this.baseUrl, data);
	}

	updateNutrition(nutritionId: number, data: any): Observable<any> {
		return this.http.put(`${this.baseUrl}/${nutritionId}`, data);
	}

	patchNutrition(nutritionId: number, data: any): Observable<any> {
		return this.http.patch(`${this.baseUrl}/${nutritionId}`, data);
	}

	deleteNutrition(nutritionId: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/nutrition/${nutritionId}`);
	}

	// New Method to Fetch Combined Recipe and Nutrition Details
	getNutritionDetailsByRecipe(recipeId: number): Observable<any> {
		return this.http.get(`${this.nutritionDetailsBaseUrl}/recipe/${recipeId}`);
	}
}
