import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class RecipeService {
	private baseUrl = 'http://localhost:5228/api/Recipe';

	constructor(private http: HttpClient) {}

	getAllRecipes(): Observable<any> {
		return this.http.get(`${this.baseUrl}`);
	}

	getRecipeById(id: number): Observable<any> {
		return this.http.get(`${this.baseUrl}/${id}`);
	}

	createRecipe(recipe: FormData): Observable<any> {
		return this.http.post(`${this.baseUrl}`, recipe);
	}

	updateRecipe(id: number, recipe: FormData): Observable<any> {
		return this.http.put(`${this.baseUrl}/${id}`, recipe);
	}

	deleteRecipe(id: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/${id}`);
	}

	patchRecipe(recipeId: number, patchData: FormData): Observable<any> {
		return this.http.patch(`${this.baseUrl}/${recipeId}`, patchData);
	}
}
