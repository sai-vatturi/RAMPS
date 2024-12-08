import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app.component';
import { routes } from './app/app.routes';
import { authInterceptor } from './app/interceptors/auth.interceptor';

bootstrapApplication(AppComponent, {
	providers: [
		provideRouter(routes),
		provideHttpClient(withInterceptors([authInterceptor])), // Use functional interceptor
		importProvidersFrom(BrowserAnimationsModule)
	]
}).catch(err => console.error(err));
