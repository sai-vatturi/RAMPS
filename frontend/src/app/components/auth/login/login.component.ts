import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, FormsModule, RouterLink],
})
export class LoginComponent {
  loginData = {
    username: '',
    password: '',
  }; // Holds the login form data
  showPassword = false; // Toggle password visibility
  alertMessage: {
    severity: 'success' | 'error' | 'info' | 'warning';
    summary: string;
    detail: string;
  } | null = null; // To display alerts

  constructor(private authService: AuthService, private router: Router) {}

  // Handle form submission
  onSubmit() {
    this.authService.login(this.loginData).subscribe({
      next: (res: any) => {
        const token = res.token;
        const expiry = res.expiry; // Assuming the backend sends token expiry

        // Store the token and expiry in localStorage
        this.authService.setToken(token, expiry);

        // Show success alert and navigate to the dashboard
        this.showAlert('success', 'Login Successful', 'Redirecting to your dashboard...');
        setTimeout(() => {
          this.router.navigate(['/user-dashboard']); // Redirect to the default dashboard
        }, 2000);
      },
      error: (err) => {
        // Handle login errors
        const errorMsg = err.error || 'Invalid username or password.';
        this.showAlert('error', 'Login Failed', errorMsg);
      },
    });
  }

  // Toggle password visibility
  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  // Display an alert
  showAlert(
    severity: 'success' | 'error' | 'info' | 'warning',
    summary: string,
    detail: string
  ) {
    this.alertMessage = { severity, summary, detail };
    // Automatically clear the alert after 5 seconds
    setTimeout(() => this.clearAlert(), 5000);
  }

  // Clear the alert message
  clearAlert() {
    this.alertMessage = null;
  }
}
