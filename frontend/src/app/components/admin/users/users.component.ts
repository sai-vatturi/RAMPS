import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
	users: any[] = [];
	selectedUser: any = null;
	action: 'approve' | 'delete' | null = null;

	// New user form data
	newUser = {
	  firstName: '',
	  lastName: '',
	  username: '',
	  email: '',
	  password: '',
	  phoneNumber: '',
	  role: 'User', // Default role
	  isApproved: true, // Default to approved
	};

	constructor(private adminService: AdminService) {
	  this.loadUsers();
	}

	loadUsers() {
	  this.adminService.getAllUsers().subscribe({
		next: (data) => {
		  this.users = data.$values || data; // Extract users from response
		  this.users.forEach(user => {
			user.updatedRole = user.role; // Initialize updatedRole for role dropdown
		  });
		},
		error: (err) => alert(`Error: ${err.error}`),
	  });
	}

	openDialog(user: any, action: 'approve' | 'delete') {
	  this.selectedUser = user;
	  this.action = action;
	}

	closeDialog() {
	  this.selectedUser = null;
	  this.action = null;
	}

	approveUser() {
	  if (this.selectedUser) {
		this.adminService.approveUser(this.selectedUser.username).subscribe({
		  next: () => {
			alert(`User ${this.selectedUser.username} approved successfully.`);
			this.closeDialog();
			this.loadUsers();
		  },
		  error: (err) => alert(`Error: ${err.error}`),
		});
	  }
	}

	deleteUser() {
	  if (this.selectedUser) {
		this.adminService.deleteUser(this.selectedUser.username).subscribe({
		  next: () => {
			alert(`User ${this.selectedUser.username} deleted successfully.`);
			this.closeDialog();
			this.loadUsers();
		  },
		  error: (err) => {
			if (err.status === 200) {
			  alert(`User ${this.selectedUser.username} deleted successfully.`);
			  this.closeDialog();
			  this.loadUsers();
			} else {
			  console.error('Unexpected error during user deletion:', err);
			}
		  },
		});
	  }
	}

	addUser() {
	  console.log('New User Payload:', this.newUser); // Log payload for debugging
	  this.adminService.addUser(this.newUser).subscribe({
		next: () => {
		  alert(`User ${this.newUser.username} added successfully.`);
		  this.newUser = {
			firstName: '',
			lastName: '',
			username: '',
			email: '',
			password: '',
			phoneNumber: '',
			role: 'User', // Reset role to default
			isApproved: true, // Reset approval to default
		  };
		  this.loadUsers(); // Refresh the user list
		},
		error: (err) => {
		  const errorMessage =
			err.error?.message || 'An unexpected error occurred.';
		  console.error('Error adding user:', err.error);
		  alert(`Error adding user: ${errorMessage}`);
		},
	  });
	}

	roleChanged(user: any) {
	  if (user.role !== user.updatedRole) {
		this.adminService.updateUserRole(user.username, user.updatedRole).subscribe({
		  next: () => {
			alert(`Role updated for user ${user.username}.`);
			this.loadUsers();
		  },
		  error: (err) => {
			const errorMessage =
			  err.error?.message || 'An unexpected error occurred.';
			console.error('Error updating role:', err.error);
			alert(`Error updating role: ${errorMessage}`);
		  },
		});
	  }
	}
  }
