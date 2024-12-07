import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private baseUrl = 'http://localhost:5228/api/Admin';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/users`);
  }

  approveUser(username: string): Observable<string> {
	return this.http.put(`${this.baseUrl}/approve-user`, { username, isApproved: true }, { responseType: 'text' });
  }


  deleteUser(username: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete-user/${username}`);
  }

  addUser(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/add-user`, user);
  }
  updateUserRole(username: string, newRole: string): Observable<any> {
	return this.http.put(`${this.baseUrl}/update-role`, { username, newRole });
  }

}
