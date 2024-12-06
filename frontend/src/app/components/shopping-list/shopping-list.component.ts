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

  constructor(private shoppingService: ShoppingService) {}

  ngOnInit(): void {
    this.loadMealPlans();
    this.loadShoppingLists();
  }

  loadMealPlans(): void {
    this.shoppingService.getMealPlans().subscribe({
      next: (data) => (this.mealPlans = data.$values || []), // Ensure $values is handled
      error: (err) => console.error('Error loading meal plans', err)
    });
  }

  loadShoppingLists(): void {
    this.shoppingService.getShoppingLists().subscribe({
      next: (data) => {
        // Transform the response to handle $values
        this.shoppingLists = (data.$values || []).map((list: any) => ({
          userShoppingListId: list.userShoppingListId,
          mealPlanId: list.mealPlanId,
          items: list.items?.$values || [] // Ensure nested $values is handled
        }));
      },
      error: (err) => console.error('Error loading shopping lists', err)
    });
  }

  generateShoppingList(): void {
    if (this.selectedMealPlanId) {
      this.shoppingService.generateShoppingList(this.selectedMealPlanId).subscribe({
        next: () => this.loadShoppingLists(),
        error: (err) => console.error('Error generating shopping list', err)
      });
    }
  }

  markAsPurchased(itemId: number): void {
    this.shoppingService.markAsPurchased(itemId).subscribe({
      next: () => this.loadShoppingLists(),
      error: (err) => console.error('Error marking item as purchased', err)
    });
  }

  unmarkAsPurchased(itemId: number): void {
    this.shoppingService.unmarkAsPurchased(itemId).subscribe({
      next: () => this.loadShoppingLists(),
      error: (err) => console.error('Error unmarking item as purchased', err)
    });
  }

  deleteItem(itemId: number): void {
    this.shoppingService.deleteItem(itemId).subscribe({
      next: () => this.loadShoppingLists(),
      error: (err) => console.error('Error deleting item', err)
    });
  }

  deleteShoppingList(listId: number): void {
	this.shoppingService.deleteShoppingList(listId).subscribe({
	  next: () => {
		console.log('Shopping list deleted successfully');
		this.loadShoppingLists(); // Refresh the shopping lists
	  },
	  error: (err) => console.error('Error deleting shopping list', err.message)
	});
  }

}
