import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
	providedIn: 'root'
})
export class RecipeService {
	private baseUrl = `${environment.apiUrl}api/Recipe`;

	constructor(private http: HttpClient) {}

	// Fetch all recipes with pagination
	getAllRecipes(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
		return this.http.get(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
	}

	// Fetch recipe count
	getRecipeCount(): Observable<any> {
		return this.http.get(`${this.baseUrl}/count`);
	}

	// Fetch a recipe by ID
	getRecipeById(id: number): Observable<any> {
		return this.http.get(`${this.baseUrl}/${id}`);
	}

	// Create a new recipe
	createRecipe(recipe: FormData): Observable<any> {
		return this.http.post(`${this.baseUrl}`, recipe);
	}

	// Update an existing recipe
	updateRecipe(id: number, recipe: FormData): Observable<any> {
		return this.http.put(`${this.baseUrl}/${id}`, recipe);
	}

	// Delete a recipe
	deleteRecipe(id: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/${id}`);
	}

	// Patch an existing recipe
	patchRecipe(recipeId: number, patchData: FormData): Observable<any> {
		return this.http.patch(`${this.baseUrl}/${recipeId}`, patchData);
	}
}
