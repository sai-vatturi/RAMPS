// landing-page.component.ts
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
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
export class LandingPageComponent implements OnInit {
	// Mobile menu toggle
	isMobileMenuOpen: boolean = false;

	// Subscription form
	subscriptionEmail: string = '';

	// Testimonials data
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

	// Current testimonial index for carousel
	currentTestimonialIndex: number = 0;

	// Timer for auto-rotation of testimonials
	testimonialInterval: any;

	ngOnInit() {
		// Start the testimonial carousel auto-rotation
		this.testimonialInterval = setInterval(() => {
			this.nextTestimonial();
		}, 5000);
	}

	// Clean up the interval on component destroy
	ngOnDestroy() {
		if (this.testimonialInterval) {
			clearInterval(this.testimonialInterval);
		}
	}

	// Toggle mobile menu
	toggleMobileMenu() {
		this.isMobileMenuOpen = !this.isMobileMenuOpen;
	}

	// Navigate to previous testimonial
	prevTestimonial() {
		this.currentTestimonialIndex = (this.currentTestimonialIndex - 1 + this.testimonials.length) % this.testimonials.length;
	}

	// Navigate to next testimonial
	nextTestimonial() {
		this.currentTestimonialIndex = (this.currentTestimonialIndex + 1) % this.testimonials.length;
	}

	// Handle subscription form submission
	subscribe() {
		if (this.subscriptionEmail) {
			// Handle subscription logic here (e.g., API call)
			alert(`Subscribed with email: ${this.subscriptionEmail}`);
			this.subscriptionEmail = '';
		}
	}
}
