import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LayoutComponent } from './layouts/layout/layout.component';

export const routes: Routes = [
	{
		path: '',
		component: LayoutComponent,
		canActivate: [AuthGuard],
		children: [
			{
				path: 'user-dashboard',
				loadComponent: () => import('./components/user-dashboard/user-dashboard.component').then(m => m.UserDashboardComponent)
			},
			{
				path: 'recipes',
				loadComponent: () => import('./components/chef/recipe/recipe.component').then(m => m.RecipeComponent)
			},
			{
				path: 'nutrition',
				loadComponent: () => import('./components/nutritionist/nutrition/nutrition.component').then(m => m.NutritionComponent)
			},
			{
				path: 'meal-plan',
				loadComponent: () => import('./components/mealplanner/meal-plan/meal-plan.component').then(m => m.MealPlanComponent)
			},
			{
				path: 'shopping-lists',
				loadComponent: () => import('./components/shopping-list/shopping-list.component').then(m => m.ShoppingComponent)
			},
			{
				path: 'users',
				loadComponent: () => import('./components/admin/users/users.component').then(m => m.UsersComponent)
			},
			{
				path: 'faq',
				loadComponent: () => import('./components/faq/faq.component').then(m => m.FaqComponent)
			}
		]
	},
	{
		path: 'signup',
		loadComponent: () => import('./components/auth/singup/signup.component').then(m => m.SignupComponent)
	},
	{
		path: 'landing-page',
		loadComponent: () => import('./components/landing-page/landing-page.component').then(m => m.LandingPageComponent)
	},
	{
		path: 'login',
		loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent)
	},
	{
		path: 'verify-email',
		loadComponent: () => import('./components/auth/verify-email/verify-email.component').then(m => m.VerifyEmailComponent)
	},
	{
		path: 'reset-password',
		loadComponent: () => import('./components/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent)
	},
	{
		path: 'request-password-reset',
		loadComponent: () => import('./components/auth/reset-password/request-password-reset.component').then(m => m.RequestPasswordResetComponent)
	},
	{ path: '**', redirectTo: 'login' }
];
