import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reset-password.component.html',
})
export class ResetPasswordComponent {
  newPassword: string = '';
  token: string | null = null;
  isLoading: boolean = false;
  alertMessage: { severity: 'success' | 'error'; message: string } | null = null;

  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {
    this.token = this.route.snapshot.queryParamMap.get('token');
  }

  // Submit the new password
  onSubmit() {
    if (this.token && this.newPassword) {
      this.isLoading = true;
      this.authService.resetPassword({ token: this.token, newPassword: this.newPassword }).subscribe({
        next: () => {
          this.showAlert('success', 'Password reset successfully!');
          setTimeout(() => this.router.navigate(['/login']), 2000); // Redirect after 2 seconds
        },
        error: (err) => {
          this.isLoading = false;
          this.showAlert('error', err.error || 'Error resetting password.');
        }
      });
    } else {
      this.showAlert('error', 'Invalid token or password.');
    }
  }

  // Show the alert message
  showAlert(severity: 'success' | 'error', message: string) {
    this.alertMessage = { severity, message };
    setTimeout(() => this.clearAlert(), 5000); // Clear alert after 5 seconds
  }

  // Clear the alert message
  clearAlert() {
    this.alertMessage = null;
  }

  // Navigate to the login page
  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  // Navigate to the signup page
  navigateToSignup() {
    this.router.navigate(['/signup']);
  }
}
