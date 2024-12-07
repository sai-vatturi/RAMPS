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

  onSubmit() {
	this.authService.login(this.loginData).subscribe({
	  next: (res: any) => {
		const token = res.token; // Token from backend
		const expiry = res.expiry; // Expiry date from backend

		// Save token and expiry in AuthService
		this.authService.setToken(token, expiry);

		// Decode token to extract role
		const payload = this.authService['decodeJwt'](token); // Use the decodeJwt method from AuthService
		const role = payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

		if (role) {
		  this.authService.setUserRole(role); // Update role in BehaviorSubject
		  localStorage.setItem('userRole', role); // Optionally save role to localStorage
		  console.log('Role extracted and saved:', role);
		} else {
		  console.error('Role is missing in token payload!');
		}

		// Display success message and redirect
		this.showAlert('success', 'Login Successful', 'Redirecting to your dashboard...');
		setTimeout(() => {
		  this.router.navigate(['/recipes']);
		}, 2000);
	  },
	  error: (err) => {
		// Handle errors
		const errorMsg = err.error?.message || 'An unexpected error occurred.';
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
