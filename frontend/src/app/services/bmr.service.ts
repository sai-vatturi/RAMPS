import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface BMRCalculatorDto {
	weight: number;
	height: number;
	age: number;
	gender: string;
	activityLevel: string;
}

@Injectable({
	providedIn: 'root'
})
export class BMRService {
	private apiUrl = environment.apiUrl;

	constructor(private http: HttpClient) {}

	calculateBMR(dto: BMRCalculatorDto): Observable<any> {
		return this.http.post<any>(`${this.apiUrl}api/bmr/calculate-bmr`, dto);
	}
}
