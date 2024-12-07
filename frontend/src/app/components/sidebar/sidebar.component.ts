import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  imports: [CommonModule, RouterLink, RouterLinkActive],
})
export class SidebarComponent implements OnInit {
  userInfo: { firstName: string; username: string; role: string } = {
    firstName: '',
    username: '',
    role: '',
  };

  @Input() isOpen = false; // Receive state from the parent
  @Output() toggle = new EventEmitter<void>(); // Notify parent of toggles
  menuItems: { label: string; icon: string; path: string }[] = []; // Add 'path' here


  // Centralized menu configuration
  private menuConfig = {
	common: [
	//   { label: 'User Dashboard', icon: 'fas fa-tachometer-alt', path: 'user-dashboard' },
	  { label: 'Recipes', icon: 'fas fa-utensils', path: 'recipes' },
	  { label: 'Nutritions', icon: 'fas fa-leaf', path: 'nutrition' },
	  { label: 'Meal Plans', icon: 'fas fa-calendar-alt', path: 'meal-plan' },
	  { label: 'Shopping Lists', icon: 'fas fa-shopping-cart', path: 'shopping-lists' },
	//   { label: 'FAQ', icon: 'fas fa-question-circle', path: 'faq' },
	],
	admin: [
	  { label: 'Users', icon: 'fas fa-users', path: 'users' },
	],
  };



  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.getUserInfo(); // Fetch user info when the component is initialized
  }


  toggleSidebar() {
    this.isOpen = !this.isOpen;
    this.toggle.emit();
  }

  closeSidebar() {
    this.isOpen = false;
    this.toggle.emit();
  }

  isActive(path: string): boolean {
    return this.router.url === `/${path}`;
  }

  getUserInfo() {
    this.authService.getUserDetails().subscribe({
      next: (data: any) => {
        this.userInfo = {
          firstName: data.firstName,
          username: data.username,
          role: data.role,
        };

        // Dynamically generate menu items based on role
        this.generateMenuItems(data.role);
      },
      error: (err) => {
        console.error('Error fetching user details:', err);
      },
    });
  }

generateMenuItems(role: string) {
	const roleSpecificMenus = role === 'Admin' ? this.menuConfig.admin : [];
	this.menuItems = [...roleSpecificMenus, ...this.menuConfig.common];
}

  logout() {
	this.authService.logout("You've been logged out.");
  }
}
