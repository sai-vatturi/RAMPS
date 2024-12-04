-- Reset identity seed for each table to default

-- Users
DBCC CHECKIDENT ('Users', RESEED, 0);

-- Recipes
DBCC CHECKIDENT ('Recipes', RESEED, 0);

-- Nutritions
DBCC CHECKIDENT ('Nutritions', RESEED, 0);

-- MealPlans
DBCC CHECKIDENT ('MealPlans', RESEED, 0);

-- ShoppingLists
DBCC CHECKIDENT ('ShoppingLists', RESEED, 0);

-- ShoppingListItems
DBCC CHECKIDENT ('ShoppingListItems', RESEED, 0);

-- Reviews
DBCC CHECKIDENT ('Reviews', RESEED, 0);
