import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './verify-email.component.html',
})
export class VerifyEmailComponent {
  isVerified: boolean | null = null;  // null means still verifying
  alertMessage: { severity: 'success' | 'error'; summary: string; detail: string } | null = null;

  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      this.verifyEmail(token);
    }
  }

  // Function to handle the email verification
  verifyEmail(token: string) {
    this.authService.verifyEmail(token).subscribe({
      next: (response: string) => {
        // Since the response is plain text, we handle it here
        this.isVerified = true;
        this.showAlert('success', 'Verification Successful', response); // Display the success message
        setTimeout(() => {
          this.router.navigate(['/login']); // Redirect to login after 2 seconds
        }, 2000);
      },
      error: (err) => {
        this.isVerified = false;
        this.showAlert('error', 'Verification Failed', err.error || 'Invalid or expired token.');
      },
    });
  }

  // Show alert message
  showAlert(severity: 'success' | 'error', summary: string, detail: string) {
    this.alertMessage = { severity, summary, detail };

    // Automatically clear alert after 5 seconds
    setTimeout(() => this.clearAlert(), 5000);
  }

  // Clear the alert message
  clearAlert() {
    this.alertMessage = null;
  }

  // Navigate to the login page
  goToLogin() {
    this.router.navigate(['/login']);
  }
}
