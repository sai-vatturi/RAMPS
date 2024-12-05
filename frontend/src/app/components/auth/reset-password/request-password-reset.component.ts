import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-request-password-reset',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './request-password-reset.component.html',
  styleUrls: ['./request-password-reset.component.css'],
})
export class RequestPasswordResetComponent {
  email: string = '';

  constructor(private authService: AuthService) {}

  onSubmit() {
    this.authService.requestPasswordReset(this.email).subscribe({
      next: () => alert('Password reset link sent to your email.'),
      error: (err) => alert(`Error: ${err.error}`),
    });
  }
}
