import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service'; // Ensure this points to your auth service

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      // User is logged in, allow access
      return true;
    } else {
      // User is not logged in, redirect to landing page or login
      this.router.navigate(['/login']); // Replace with '/login' if preferred
      return false;
    }
  }
}
