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
	imports: [CommonModule, FormsModule, RouterLink]
})
export class LoginComponent {
	loginData = {
		username: '',
		password: ''
	};
	showPassword = false;
	alertMessage: {
		severity: 'success' | 'error' | 'info' | 'warning';
		summary: string;
		detail: string;
	} | null = null;

	constructor(private authService: AuthService, private router: Router) {}

	onSubmit() {
		this.authService.login(this.loginData).subscribe({
			next: (res: any) => {
				const token = res.token;
				const expiry = res.expiry;

				this.authService.setToken(token, expiry);

				const payload = this.authService['decodeJwt'](token);
				const role = payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

				if (role) {
					this.authService.setUserRole(role);
					localStorage.setItem('userRole', role);
					console.log('Role extracted and saved:', role);
				} else {
					console.error('Role is missing in token payload!');
				}

				this.showAlert('success', 'Login Successful', 'Redirecting to your dashboard...');
				setTimeout(() => {
					this.router.navigate(['/recipes']);
				}, 2000);
			},
			error: err => {
				const errorMsg = err.error?.message || 'An unexpected error occurred.';
				this.showAlert('error', 'Login Failed', errorMsg);
			}
		});
	}

	togglePasswordVisibility() {
		this.showPassword = !this.showPassword;
	}

	showAlert(severity: 'success' | 'error' | 'info' | 'warning', summary: string, detail: string) {
		this.alertMessage = { severity, summary, detail };
		setTimeout(() => this.clearAlert(), 5000);
	}

	clearAlert() {
		this.alertMessage = null;
	}
}
