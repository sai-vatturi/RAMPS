import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './welcome.component.html',
})
export class WelcomeComponent {
  username: string | null = null;
  role: string | null = null;

  constructor(private router: Router) {
    const token = localStorage.getItem('token');
    if (token) {
      // Decode the JWT token to extract claims
      const payload = JSON.parse(atob(token.split('.')[1]));
      this.username = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
      this.role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
