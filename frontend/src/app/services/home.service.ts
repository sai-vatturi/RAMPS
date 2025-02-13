import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
	providedIn: 'root'
})
export class HomeService {
	private apiUrl = environment.apiUrl;

	constructor(private http: HttpClient) {}

	getCurrentDayPlan(date: string): Observable<any> {
		// Format the date to include time and encode it properly
		const formattedDate = `${date}T00:00:00.000`;
		const encodedDate = encodeURIComponent(formattedDate);
		return this.http.get(`${this.apiUrl}api/HomePage/current?date=${encodedDate}`);
	}

	getDateRangePlan(startDate: string, endDate: string): Observable<any> {
		return this.http.get(`${this.apiUrl}api/HomePage/range?startDate=${startDate}&endDate=${endDate}`);
	}
}
