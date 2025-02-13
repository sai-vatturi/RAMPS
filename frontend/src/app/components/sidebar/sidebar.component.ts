import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../services/auth.service';

@Component({
	selector: 'app-sidebar',
	standalone: true,
	templateUrl: './sidebar.component.html',
	styleUrls: ['./sidebar.component.css'],
	imports: [CommonModule, RouterLink, RouterLinkActive]
})
export class SidebarComponent implements OnInit, AfterViewInit, OnDestroy {
	userInfo: { firstName: string; username: string; role: string } = {
		firstName: '',
		username: '',
		role: ''
	};

	@Input() isOpen = false;
	@Output() toggle = new EventEmitter<void>();
	menuItems: { label: string; icon: string; path: string }[] = [];

	private menuConfig = {
		common: [
			{ label: 'Home', icon: 'fas fa-home', path: 'home' }, // Add this as the first item
			{ label: 'Recipes', icon: 'fas fa-utensils', path: 'recipes' },
			{ label: 'Nutritions', icon: 'fas fa-leaf', path: 'nutrition' },
			{ label: 'Meal Plans', icon: 'fas fa-calendar-alt', path: 'meal-plan' },
			{ label: 'Shopping Lists', icon: 'fas fa-shopping-cart', path: 'shopping-lists' },
			{ label: 'Dietary Preferences', icon: 'fas fa-note-sticky', path: 'dietary-preferences' },
			{ label: 'AI Ask', icon: 'fas fa-note-sticky', path: 'ai-food-recommendation' }
		],
		admin: [{ label: 'Users', icon: 'fas fa-users', path: 'users' }]
	};

	showPaymentModal: boolean = false;

	private userRoleSubscription!: Subscription;

	constructor(private authService: AuthService, private router: Router) {}

	ngOnInit(): void {
		this.getUserInfo();
	}

	ngAfterViewInit(): void {}

	toggleSidebar() {
		this.isOpen = !this.isOpen;
		this.toggle.emit();
	}

	closeSidebar() {
		this.isOpen = false;
		this.toggle.emit();
	}

	isActive(path: string): boolean {
		return this.router.url === `/layout/${path}`;
	}

	getUserInfo() {
		this.authService.getUserDetails().subscribe({
			next: (data: any) => {
				this.userInfo = {
					firstName: data.firstName,
					username: data.username,
					role: data.role
				};

				this.generateMenuItems(data.role);
			},
			error: err => {
				console.error('Error fetching user details:', err);
			}
		});

		this.userRoleSubscription = this.authService.userRole$.subscribe(role => {
			this.userInfo.role = role;
			this.generateMenuItems(role);
		});
	}

	generateMenuItems(role: string) {
		const roleSpecificMenus = role === 'Admin' ? this.menuConfig.admin : [];
		this.menuItems = [...roleSpecificMenus, ...this.menuConfig.common];
	}

	logout() {
		this.authService.logout("You've been logged out.");
	}

	onDietaryPreferencesClick(event: Event) {
		event.preventDefault();

		console.log('Dietary Preferences clicked');
		console.log('User role:', this.userInfo.role);

		if (this.userInfo.role === 'Admin') {
			console.log('Admin detected, navigating to /layout/dietary-preferences');
			this.router.navigate(['/layout/dietary-preferences']);
			this.closeSidebar();
		} else {
			console.log('Non-admin user, showing PayPal payment modal');
			this.showPaymentModal = true;

			setTimeout(() => {
				this.renderPayPalButton();
			}, 0);
		}
	}

	renderPayPalButton() {
		if (!this.showPaymentModal) {
			return;
		}

		if ((window as any).paypal) {
			(window as any).paypal
				.Buttons({
					createOrder: (data: any, actions: any) => {
						return actions.order.create({
							purchase_units: [
								{
									amount: {
										value: '1.00' // Minimal amount as PayPal not allow 0 rs
									}
								}
							]
						});
					},
					onApprove: (data: any, actions: any) => {
						return actions.order.capture().then((details: any) => {
							console.log('Transaction completed by ' + details.payer.name.given_name);
							this.showPaymentModal = false;
							this.router.navigate(['/layout/dietary-preferences']);
							this.closeSidebar();
						});
					},
					onError: (err: any) => {
						console.error('PayPal Checkout onError', err);
					}
				})
				.render('#paypal-button-container');
		} else {
			console.error('PayPal SDK not loaded.');
		}
	}

	closePaymentModal() {
		this.showPaymentModal = false;
	}

	ngOnDestroy(): void {
		if (this.userRoleSubscription) {
			this.userRoleSubscription.unsubscribe();
		}
	}
}
