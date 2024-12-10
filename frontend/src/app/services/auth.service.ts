import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	private baseUrl = `${environment.apiUrl}api/Auth`;

	private userRoleSubject = new BehaviorSubject<string>('User');
	userRole$: Observable<string> = this.userRoleSubject.asObservable();

	constructor(private http: HttpClient) {}

	isLoggedIn(): boolean {
		const token = localStorage.getItem('token');
		const expiry = localStorage.getItem('tokenExpiry');

		if (!token || !expiry) {
			return false;
		}

		const now = new Date().getTime();
		const expiryTime = new Date(expiry).getTime();

		if (now > expiryTime) {
			this.logout('Your session has expired. Please log in again.');
			return false;
		}

		return true;
	}

	private decodeJwt(token: string): any {
		try {
			const base64Url = token.split('.')[1];
			const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
			const jsonPayload = decodeURIComponent(
				atob(base64)
					.split('')
					.map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
					.join('')
			);
			return JSON.parse(jsonPayload);
		} catch (error) {
			console.error('Error decoding token:', error);
			return null;
		}
	}

	setUserRole(role: string) {
		this.userRoleSubject.next(role);
	}

	getUserDetails(): Observable<any> {
		return this.http.get(`${this.baseUrl}/me`);
	}

	getUserRole(): Observable<string> {
		return this.userRole$;
	}

	signup(data: any): Observable<string> {
		return this.http.post(`${this.baseUrl}/signup`, data, { responseType: 'text' });
	}

	verifyEmail(token: string): Observable<string> {
		return this.http.get(`${this.baseUrl}/verify-email?token=${token}`, { responseType: 'text' });
	}

	login(data: any): Observable<any> {
		return this.http.post(`${this.baseUrl}/login`, data);
	}

	logout(message?: string): void {
		localStorage.removeItem('token');
		localStorage.removeItem('tokenExpiry');
		window.location.href = '';
	}

	requestPasswordReset(email: string): Observable<string> {
		return this.http.post(`${this.baseUrl}/request-password-reset`, { email }, { responseType: 'text' });
	}

	resetPassword(data: any): Observable<string> {
		return this.http.post(`${this.baseUrl}/reset-password`, data, { responseType: 'text' });
	}

	setToken(token: string, expiry: string): void {
		localStorage.setItem('token', token);
		localStorage.setItem('tokenExpiry', expiry);

		const payload = this.decodeJwt(token);
		const role = payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

		if (role) {
			this.setUserRole(role);
			localStorage.setItem('userRole', role);
		} else {
			console.error('Role not found in token payload.');
		}
	}
}
