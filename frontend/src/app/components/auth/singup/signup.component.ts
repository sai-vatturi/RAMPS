import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './signup.component.html',
})
export class SignupComponent {
  signupData = {
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: 'User',
  };

  constructor(private authService: AuthService) {}

  onSubmit() {
    this.authService.signup(this.signupData).subscribe({
      next: () => alert('Signup successful! Check your email for verification.'),
      error: (err) => alert(`Error: ${err.error}`),
    });
  }
}
