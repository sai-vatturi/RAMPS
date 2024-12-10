import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ShoppingService } from '../../services/shopping-list.service';

@Component({
	selector: 'app-shopping',
	templateUrl: './shopping-list.component.html',
	standalone: true,
	imports: [CommonModule, FormsModule]
})
export class ShoppingComponent implements OnInit {
	mealPlans: any[] = [];
	shoppingLists: any[] = [];
	selectedMealPlanId: number | null = null;
	selectedShoppingList: any | null = null;

	constructor(private shoppingService: ShoppingService) {}

	ngOnInit(): void {
		this.loadMealPlans();
		this.loadShoppingLists();
	}

	loadMealPlans(): void {
		this.shoppingService.getMealPlans().subscribe({
			next: data => (this.mealPlans = data.$values || []),
			error: err => console.error('Error loading meal plans', err)
		});
	}

	getMealPlanName(mealPlanId: number): string {
		const mealPlan = this.mealPlans.find(plan => plan.mealPlanId === mealPlanId);
		return mealPlan ? mealPlan.name : 'Unknown';
	}

	loadShoppingLists(): void {
		this.shoppingService.getShoppingLists().subscribe({
			next: data => {
				this.shoppingLists = (data.$values || []).map((list: any) => ({
					userShoppingListId: list.userShoppingListId,
					mealPlanId: list.mealPlanId,
					items: list.items?.$values || []
				}));
			},
			error: err => console.error('Error loading shopping lists', err)
		});
	}

	generateShoppingList(): void {
		if (this.selectedMealPlanId) {
			this.shoppingService.generateShoppingList(this.selectedMealPlanId).subscribe({
				next: response => {
					this.loadShoppingLists();
					window.alert(response.message || 'Shopping list generated successfully and emailed.');
				},
				error: err => {
					console.error('Error generating shopping list', err);
					window.alert(err.error?.message || 'Error generating shopping list.');
				}
			});
		} else {
			window.alert('Please select a meal plan.');
		}
	}

	markAsPurchased(itemId: number, item: any): void {
		this.shoppingService.markAsPurchased(itemId).subscribe({
			next: () => {
				item.isPurchased = true; // Update local state
			},
			error: err => console.error('Error marking item as purchased', err)
		});
	}

	unmarkAsPurchased(itemId: number, item: any): void {
		this.shoppingService.unmarkAsPurchased(itemId).subscribe({
			next: () => {
				item.isPurchased = false; // Update local state
			},
			error: err => console.error('Error unmarking item as purchased', err)
		});
	}

	deleteItem(itemId: number): void {
		this.shoppingService.deleteItem(itemId).subscribe({
			next: () => {
				this.selectedShoppingList.items = this.selectedShoppingList.items.filter((item: any) => item.userShoppingListItemId !== itemId);
			},
			error: err => console.error('Error deleting item', err)
		});
	}

	deleteShoppingList(listId: number): void {
		this.shoppingService.deleteShoppingList(listId).subscribe({
			next: () => {
				this.shoppingLists = this.shoppingLists.filter((list: any) => list.userShoppingListId !== listId);
			},
			error: err => console.error('Error deleting shopping list', err.message)
		});
	}

	openModal(shoppingList: any): void {
		this.selectedShoppingList = shoppingList;
	}

	closeModal(): void {
		this.selectedShoppingList = null;
	}
}
