import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-request-password-reset',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './request-password-reset.component.html',
})
export class RequestPasswordResetComponent {
  email: string = '';
  isLoading: boolean = false;
  alertMessage: {
    type: 'success' | 'error';
    message: string;
  } | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit() {
	this.isLoading = true;
	this.alertMessage = null;

	this.authService.requestPasswordReset(this.email).subscribe({
	  next: (response: string) => {
		this.isLoading = false;
		this.alertMessage = {
		  type: 'success',
		  message: response, // This will now correctly show the plain text response
		};
		setTimeout(() => this.closeAlert(), 5000);
	  },
	  error: (err) => {
		this.isLoading = false;
		this.alertMessage = {
		  type: 'error',
		  message: err.error || 'An unexpected error occurred', // Handle errors properly
		};
		setTimeout(() => this.closeAlert(), 5000);
	  },
	});
  }



  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  navigateToSignup() {
    this.router.navigate(['/signup']);
  }

  closeAlert() {
    this.alertMessage = null;
  }
}
