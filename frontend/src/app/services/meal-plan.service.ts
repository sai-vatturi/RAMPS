import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class MealPlanService {
	private baseUrl = 'http://15.207.100.163:8080/api/MealPlan';
	private recipesUrl = 'http://15.207.100.163:8080/api/Recipe';

	constructor(private http: HttpClient) {}

	getAllMealPlans(): Observable<any> {
		return this.http.get<any>(`${this.baseUrl}`);
	}

	getMealPlanById(id: number): Observable<any> {
		return this.http.get<any>(`${this.baseUrl}/${id}`);
	}

	createMealPlan(data: any): Observable<any> {
		return this.http.post<any>(`${this.baseUrl}`, data);
	}

	updateMealPlan(id: number, data: any): Observable<any> {
		return this.http.put<any>(`${this.baseUrl}/${id}`, data);
	}

	deleteMealPlan(id: number): Observable<any> {
		return this.http.delete<any>(`${this.baseUrl}/${id}`);
	}

	getAvailableRecipes(): Observable<any> {
		return this.http.get<any>(`${this.recipesUrl}`);
	}
}
