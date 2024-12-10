import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
	providedIn: 'root'
})
export class ShoppingService {
	private baseUrl = `${environment.apiUrl}api`;

	constructor(private http: HttpClient) {}

	getMealPlans(): Observable<any> {
		return this.http.get(`${this.baseUrl}/MealPlan`);
	}

	generateShoppingList(mealPlanId: number): Observable<any> {
		return this.http.post(`${this.baseUrl}/Shopping/generate?mealPlanId=${mealPlanId}`, {});
	}

	getShoppingLists(): Observable<any> {
		return this.http.get(`${this.baseUrl}/Shopping`);
	}

	markAsPurchased(itemId: number): Observable<any> {
		return this.http.put(`${this.baseUrl}/Shopping/mark-purchased/${itemId}`, {}, { responseType: 'text' });
	}

	unmarkAsPurchased(itemId: number): Observable<any> {
		return this.http.put(`${this.baseUrl}/Shopping/unmark-purchased/${itemId}`, {}, { responseType: 'text' });
	}

	deleteItem(itemId: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/Shopping/delete-item/${itemId}`, { responseType: 'text' });
	}

	deleteShoppingList(listId: number): Observable<any> {
		return this.http.delete(`${this.baseUrl}/Shopping/delete-list/${listId}`, { responseType: 'text' });
	}
}
