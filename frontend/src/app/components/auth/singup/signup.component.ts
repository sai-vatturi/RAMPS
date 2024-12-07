import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

interface AlertMessage {
  severity: 'success' | 'error' | 'info' | 'warning';
  summary: string;
  detail: string;
}

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './signup.component.html',
})
export class SignupComponent implements OnInit {
  signupData = {
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: 'User',
  };

  showPassword = false;
  alertMessage: AlertMessage | null = null;
  isLoading = false;

  roles = [
    { value: 'Admin', label: 'Admin' },
    { value: 'Chef', label: 'Chef' },
    { value: 'Nutritionist', label: 'Nutritionist' },
    { value: 'MealPlanner', label: 'Meal Planner' },
    { value: 'User', label: 'User' }
  ];

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.clearAlert();
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    this.isLoading = true;
    this.authService.signup(this.signupData).subscribe({
      next: (response: string) => {  // Now response is a string (plain text)
        this.showAlert('success', 'Signup Successful', response);  // Handle it as a plain text message
        setTimeout(() => this.router.navigate(['/login']), 5000); // Redirect to login page after 5 seconds
      },
      error: (err) => {
        this.isLoading = false;
        this.showAlert('error', 'Signup Failed', err.error || 'An error occurred during signup.');
      }
    });
  }

  showAlert(severity: AlertMessage['severity'], summary: string, detail: string) {
    this.alertMessage = { severity, summary, detail };

    // Automatically clear alert after 5 seconds
    setTimeout(() => this.clearAlert(), 5000);
  }

  clearAlert() {
    this.alertMessage = null;
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }
}
