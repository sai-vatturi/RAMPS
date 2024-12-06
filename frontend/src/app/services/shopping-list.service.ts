import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShoppingService {
  private baseUrl = 'http://localhost:5228/api'; // Update with your backend URL

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
