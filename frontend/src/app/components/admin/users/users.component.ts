import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';

@Component({
	selector: 'app-users',
	standalone: true,
	imports: [CommonModule, FormsModule],
	templateUrl: './users.component.html',
	styleUrls: ['./users.component.css']
})
export class UsersComponent {
	users: any[] = [];
	pendingUsers: any[] = [];
	approvedUsers: any[] = [];
	selectedUser: any = null;
	action: 'approve' | 'delete' | null = null;

	isAddUserDialogOpen: boolean = false;
	isActionDialogOpen: boolean = false;
	isChangeRoleDialogOpen: boolean = false;

	newUser = {
		firstName: '',
		lastName: '',
		username: '',
		email: '',
		password: '',
		phoneNumber: '',
		role: 'User',
		isApproved: false
	};

	newRole: string = '';

	constructor(private adminService: AdminService) {
		this.loadUsers();
	}

	loadUsers() {
		this.adminService.getAllUsers().subscribe({
			next: data => {
				this.users = data.$values || data;
				this.pendingUsers = this.users.filter(u => !u.isApproved);
				this.approvedUsers = this.users.filter(u => u.isApproved);
			},
			error: err => console.error(`Error loading users: ${err.message}`)
		});
	}

	openAddUserDialog() {
		this.isAddUserDialogOpen = true;
	}

	closeAddUserDialog() {
		this.isAddUserDialogOpen = false;
		this.resetNewUser();
	}

	resetNewUser() {
		this.newUser = {
			firstName: '',
			lastName: '',
			username: '',
			email: '',
			password: '',
			phoneNumber: '',
			role: 'User',
			isApproved: false
		};
	}

	addUser() {
		this.adminService.addUser(this.newUser).subscribe({
			next: () => {
				alert(`User ${this.newUser.username} added successfully.`);
				this.closeAddUserDialog();
				this.loadUsers();
			},
			error: err => console.error(`Error adding user: ${err.message}`)
		});
	}
	openActionDialog(user: any, action: 'approve' | 'delete') {
		this.selectedUser = user;
		this.action = action;
		this.isActionDialogOpen = true;
	}

	closeActionDialog() {
		this.selectedUser = null;
		this.action = null;
		this.isActionDialogOpen = false;
	}

	approveUser() {
		if (this.selectedUser) {
			this.adminService.approveUser(this.selectedUser.username).subscribe({
				next: (response: string) => {
					alert(response || `User ${this.selectedUser.username} approved successfully.`);
					this.closeActionDialog();
					this.loadUsers();
				},
				error: err => {
					console.error(`Error approving user: ${err.message}`);
					alert(`Failed to approve user: ${err.message}`);
				}
			});
		}
	}

	deleteUser() {
		if (this.selectedUser) {
			this.adminService.deleteUser(this.selectedUser.username).subscribe({
				next: () => {
					alert(`User ${this.selectedUser.username} deleted successfully.`);
					this.closeActionDialog();
					this.loadUsers();
				},
				error: err => console.error(`Error deleting user: ${err.message}`)
			});
		}
	}

	openChangeRoleDialog(user: any) {
		this.selectedUser = user;
		this.newRole = user.role;
		this.isChangeRoleDialogOpen = true;
	}

	closeChangeRoleDialog() {
		this.selectedUser = null;
		this.newRole = '';
		this.isChangeRoleDialogOpen = false;
	}

	confirmChangeRole() {
		if (this.selectedUser && this.newRole && this.newRole !== this.selectedUser.role) {
			this.adminService.updateUserRole(this.selectedUser.username, this.newRole).subscribe({
				next: () => {
					alert(`Role updated for user ${this.selectedUser.username} to ${this.newRole}.`);
					this.closeChangeRoleDialog();
					this.loadUsers();
				},
				error: err => console.error(`Error updating role: ${err.message}`)
			});
		} else {
			alert('No changes made to the role.');
			this.closeChangeRoleDialog();
		}
	}
}
