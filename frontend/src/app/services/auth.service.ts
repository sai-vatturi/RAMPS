import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'http://localhost:5228/api/Auth'; // Update this URL based on your backend

  private userRoleSubject = new BehaviorSubject<string>('User'); // Default role
  userRole$: Observable<string> = this.userRoleSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Check if the user is logged in
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

  // Decode JWT and extract payload
private decodeJwt(token: string): any {
	try {
	  const base64Url = token.split('.')[1]; // Get the payload part of the token
	  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
	  const jsonPayload = decodeURIComponent(
		atob(base64)
		  .split('')
		  .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
		  .join('')
	  );
	  return JSON.parse(jsonPayload);
	} catch (error) {
	  console.error('Error decoding token:', error);
	  return null;
	}
  }



  // Set user role in BehaviorSubject
  setUserRole(role: string) {
    this.userRoleSubject.next(role);
  }

  // Get user details from backend
  getUserDetails(): Observable<any> {
    return this.http.get(`${this.baseUrl}/me`); // Assuming a "me" endpoint is present
  }

  // Get user role
  getUserRole(): Observable<string> {
    return this.userRole$;
  }

  // Signup method
  signup(data: any): Observable<string> {
    return this.http.post(`${this.baseUrl}/signup`, data, { responseType: 'text' });
  }

  // Verify email
  verifyEmail(token: string): Observable<string> {
    return this.http.get(`${this.baseUrl}/verify-email?token=${token}`, { responseType: 'text' });
  }

  // Login method
  login(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, data);
  }

  // Logout
  // Logout user by calling the backend endpoint
  logout(message?: string): void {
	// Clear token and other user-related data
	localStorage.removeItem('token');
	localStorage.removeItem('tokenExpiry');

	// Redirect to homepage or login page
	window.location.href = '/login';
  }


  // Request password reset
  requestPasswordReset(email: string): Observable<string> {
    return this.http.post(`${this.baseUrl}/request-password-reset`, { email }, { responseType: 'text' });
  }

  // Reset password
  resetPassword(data: any): Observable<string> {
    return this.http.post(`${this.baseUrl}/reset-password`, data, { responseType: 'text' });
  }

  // Set token and expiry in localStorage
  // Set token and expiry in localStorage, and extract the role
setToken(token: string, expiry: string): void {
	localStorage.setItem('token', token);
	localStorage.setItem('tokenExpiry', expiry); // Expiry should be a UTC string

	// Decode token to extract role
	const payload = this.decodeJwt(token);
	const role = payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

	if (role) {
	  this.setUserRole(role); // Update the BehaviorSubject
	  localStorage.setItem('userRole', role); // Optional: Store role in localStorage
	} else {
	  console.error('Role not found in token payload.');
	}
  }

}
