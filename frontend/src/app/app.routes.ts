import { Routes } from '@angular/router';
import { UsersComponent } from './components/admin/users/users.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RequestPasswordResetComponent } from './components/auth/reset-password/request-password-reset.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { SignupComponent } from './components/auth/singup/signup.component';
import { VerifyEmailComponent } from './components/auth/verify-email/verify-email.component';
import { RecipeComponent } from './components/chef/recipe/recipe.component';
import { FaqComponent } from './components/faq/faq.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { MealPlanComponent } from './components/mealplanner/meal-plan/meal-plan.component';
import { NutritionComponent } from './components/nutritionist/nutrition/nutrition.component';
import { ShoppingComponent } from './components/shopping-list/shopping-list.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { AuthGuard } from './guards/auth.guard'; // Import the AuthGuard
import { LayoutComponent } from './layouts/layout/layout.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'user-dashboard', component: UserDashboardComponent, canActivate: [AuthGuard] }, // Protected
      { path: 'recipes', component: RecipeComponent, canActivate: [AuthGuard] }, // Protected
      { path: 'nutrition', component: NutritionComponent, canActivate: [AuthGuard] }, // Protected
      { path: 'meal-plan', component: MealPlanComponent, canActivate: [AuthGuard] }, // Protected
      { path: 'shopping-lists', component: ShoppingComponent, canActivate: [AuthGuard] }, // Protected
      { path: 'users', component: UsersComponent, canActivate: [AuthGuard] }, // Protected
      { path: 'faq', component: FaqComponent, canActivate: [AuthGuard] }, // Protected
    ],
  },
  { path: 'signup', component: SignupComponent }, // Public
  { path: 'landing-page', component: LandingPageComponent }, // Public
  { path: 'login', component: LoginComponent }, // Public
  { path: 'verify-email', component: VerifyEmailComponent }, // Public
  { path: 'reset-password', component: ResetPasswordComponent }, // Public
  { path: 'request-password-reset', component: RequestPasswordResetComponent }, // Public
  { path: 'sidebar', component: SidebarComponent, canActivate: [AuthGuard] }, // Protected
  { path: '**', redirectTo: 'login' }, // Redirect unmatched paths to login
];
