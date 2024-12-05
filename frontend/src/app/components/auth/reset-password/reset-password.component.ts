import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private route: ActivatedRoute, private authService: AuthService) {
    this.token = this.route.snapshot.queryParamMap.get('token');
  }

  onSubmit() {
    if (this.token) {
      this.authService.resetPassword({ token: this.token, newPassword: this.newPassword }).subscribe({
        next: () => alert('Password reset successfully!'),
        error: (err) => alert(`Error: ${err.error}`),
      });
    } else {
      alert('Invalid token.');
    }
  }
}
