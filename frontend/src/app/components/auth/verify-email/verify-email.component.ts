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
  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      this.authService.verifyEmail(token).subscribe({
        next: () => {
          alert('Email verified successfully!');
          this.router.navigate(['/login']);
        },
        error: (err) => alert(`Error: ${err.error}`),
      });
    }
  }
}
