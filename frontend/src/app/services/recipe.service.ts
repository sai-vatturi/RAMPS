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
