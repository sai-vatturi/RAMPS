import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
	standalone: true,
	imports: [FormsModule],
	selector: 'app-landing-page',
	templateUrl: './landing-page.component.html',
	styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent {
	menuItems = [
		{ label: 'Features', command: () => this.scrollToSection('features') },
		{ label: 'Plans', command: () => this.scrollToSection('plans') },
		{ label: 'Contact', command: () => this.scrollToSection('contact') }
	];

	scrollToSection(sectionId: string) {
		const el = document.getElementById(sectionId);
		if (el) el.scrollIntoView({ behavior: 'smooth', block: 'start' });
	}
}
