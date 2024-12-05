import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NutritionService {
  private baseUrl = 'http://localhost:5228/api/Nutrition';

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
  deleteNutrition(nutritionId: number) {
	return this.http.delete(`${this.baseUrl}/nutrition/${nutritionId}`);
  }

}
