import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
	selector: 'app-verify-email',
	standalone: true,
	imports: [CommonModule],
	templateUrl: './verify-email.component.html'
})
export class VerifyEmailComponent {
	isVerified: boolean | null = null;
	alertMessage: { severity: 'success' | 'error'; summary: string; detail: string } | null = null;

	constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {
		const token = this.route.snapshot.queryParamMap.get('token');
		if (token) {
			this.verifyEmail(token);
		}
	}

	verifyEmail(token: string) {
		this.authService.verifyEmail(token).subscribe({
			next: (response: string) => {
				this.isVerified = true;
				this.showAlert('success', 'Verification Successful', response);
				setTimeout(() => {
					this.router.navigate(['/login']);
				}, 2000);
			},
			error: err => {
				this.isVerified = false;
				this.showAlert('error', 'Verification Failed', err.error || 'Invalid or expired token.');
			}
		});
	}

	showAlert(severity: 'success' | 'error', summary: string, detail: string) {
		this.alertMessage = { severity, summary, detail };

		setTimeout(() => this.clearAlert(), 5000);
	}

	clearAlert() {
		this.alertMessage = null;
	}

	goToLogin() {
		this.router.navigate(['/login']);
	}
}
