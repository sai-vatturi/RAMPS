import { CommonModule } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

interface Testimonial {
	name: string;
	role: string;
	comment: string;
	avatar: string;
}

@Component({
	selector: 'app-landing-page',
	standalone: true,
	imports: [CommonModule, FormsModule, RouterModule],
	templateUrl: './landing-page.component.html',
	styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent implements OnInit, OnDestroy {
	isMobileMenuOpen: boolean = false;
	isScrolled = false;
	tagline: string = 'Plan Your Perfect Meals, Track Your Health, Eat Well!';
	description: string = 'Take control of your nutrition journey with our comprehensive meal planning and health tracking platform.';
	subscriptionEmail: string = '';

	@HostListener('window:scroll', [])
	onWindowScroll() {
		const heroHeight = document.getElementById('hero')?.offsetHeight || 0;
		this.isScrolled = window.scrollY > heroHeight;
	}

	testimonials: Testimonial[] = [
		{
			name: 'Jane Doe',
			role: 'Chef',
			comment: "FoodPro has completely transformed the way I plan my meals. It's intuitive and easy to use!",
			avatar: 'https://via.placeholder.com/64'
		},
		{
			name: 'John Smith',
			role: 'Nutritionist',
			comment: 'The personalized meal plans helped me achieve my dietary goals effortlessly.',
			avatar: 'https://via.placeholder.com/64'
		},
		{
			name: 'Emily Johnson',
			role: 'Home Cook',
			comment: 'The shopping list feature saves me so much time at the grocery store. Highly recommend!',
			avatar: 'https://via.placeholder.com/64'
		}
	];

	currentTestimonialIndex: number = 0;
	testimonialInterval: any;

	ngOnInit() {}

	ngOnDestroy() {
		if (this.testimonialInterval) {
			clearInterval(this.testimonialInterval);
		}
	}

	toggleMobileMenu() {
		this.isMobileMenuOpen = !this.isMobileMenuOpen;
	}

	subscribe() {
		if (this.subscriptionEmail) {
			alert(`Subscribed with email: ${this.subscriptionEmail}`);
			this.subscriptionEmail = '';
		}
	}
}
