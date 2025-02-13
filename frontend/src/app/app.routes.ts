import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LayoutComponent } from './layouts/layout/layout.component';

export const routes: Routes = [
	{
		path: 'login',
		loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent)
	},

	{
		path: 'layout',
		component: LayoutComponent,
		canActivate: [AuthGuard],
		children: [
			{ path: '', redirectTo: 'recipes', pathMatch: 'full' },
			{ path: 'recipes', loadComponent: () => import('./components/chef/recipe/recipe.component').then(m => m.RecipeComponent) },
			{ path: 'nutrition', loadComponent: () => import('./components/nutritionist/nutrition/nutrition.component').then(m => m.NutritionComponent) },
			{ path: 'meal-plan', loadComponent: () => import('./components/mealplanner/meal-plan/meal-plan.component').then(m => m.MealPlanComponent) },
			{ path: 'shopping-lists', loadComponent: () => import('./components/shopping-list/shopping-list.component').then(m => m.ShoppingComponent) },
			{ path: 'dietary-preferences', loadComponent: () => import('./components/dietary-preferences/dietary-preferences.component').then(m => m.DietaryPreferencesComponent) },
			{ path: 'users', loadComponent: () => import('./components/admin/users/users.component').then(m => m.UsersComponent) },
			{ path: 'ai-food-recommendation', loadComponent: () => import('./components/ai-food-recommendation/ai-food-recommendation.component').then(m => m.AiFoodRecommendationComponent) },
			{ path: 'home', loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent) }
		]
	},

	{ path: 'signup', loadComponent: () => import('./components/auth/singup/signup.component').then(m => m.SignupComponent) },
	{ path: 'verify-email', loadComponent: () => import('./components/auth/verify-email/verify-email.component').then(m => m.VerifyEmailComponent) },
	{ path: 'reset-password', loadComponent: () => import('./components/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) },
	{ path: 'request-password-reset', loadComponent: () => import('./components/auth/reset-password/request-password-reset.component').then(m => m.RequestPasswordResetComponent) },
	{ path: '', loadComponent: () => import('./components/landing-page/landing-page.component').then(m => m.LandingPageComponent) },
	{ path: '**', redirectTo: '' }
];
