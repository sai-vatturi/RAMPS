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
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { LayoutComponent } from './layouts/layout/layout.component';

export const routes: Routes = [
	{
		path: '',
		component: LayoutComponent,
		canActivate: [AuthGuard], // Protect the LayoutComponent itself
		children: [
			{ path: 'user-dashboard', component: UserDashboardComponent },
			{ path: 'recipes', component: RecipeComponent },
			{ path: 'nutrition', component: NutritionComponent },
			{ path: 'meal-plan', component: MealPlanComponent },
			{ path: 'shopping-lists', component: ShoppingComponent },
			{ path: 'users', component: UsersComponent },
			{ path: 'faq', component: FaqComponent },
		],
	},
	{ path: 'signup', component: SignupComponent }, // Public
	{ path: 'landing-page', component: LandingPageComponent }, // Public
	{ path: 'login', component: LoginComponent }, // Public
	{ path: 'verify-email', component: VerifyEmailComponent }, // Public
	{ path: 'reset-password', component: ResetPasswordComponent }, // Public
	{ path: 'request-password-reset', component: RequestPasswordResetComponent }, // Public
	{ path: '**', redirectTo: 'login' }, // Redirect unmatched paths to login
];
