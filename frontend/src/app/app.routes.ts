import { Routes } from '@angular/router';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RequestPasswordResetComponent } from './components/auth/reset-password/request-password-reset.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { SignupComponent } from './components/auth/singup/signup.component';
import { VerifyEmailComponent } from './components/auth/verify-email/verify-email.component';
import { RecipeComponent } from './components/chef/recipe/recipe.component';
import { NutritionComponent } from './components/nutritionist/nutrition/nutrition.component';
import { WelcomeComponent } from './components/welcome/welcome.component';

export const routes: Routes = [
  { path: 'signup', component: SignupComponent },
  { path: 'login', component: LoginComponent },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'request-password-reset', component: RequestPasswordResetComponent },
  { path: 'welcome', component: WelcomeComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'recipes', component: RecipeComponent },
  { path: 'nutrition', component: NutritionComponent },
  { path: '**', redirectTo: 'login' },
];
