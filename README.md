
# Recipe and Meal Planning Management System


<img width="1333" alt="Screenshot 2024-12-09 at 11 28 10 AM" src="https://github.com/user-attachments/assets/914f9d9a-75cd-4f99-b1c1-468a204b11fd">

Host link: https://sai-vatturi.github.io/host-link/ (Dont change admin user role)
## Overview
The Recipe and Meal Planning Management System is a modern web application that simplifies meal planning, recipe management, and dietary tracking for various users, including Admins, Chefs, Nutritionists, and Customers. Built using ASP.NET Core and Angular, it provides a seamless and interactive experience for users.
## Features

### Backend Features
- **User Authentication and Role Management**
  - JWT-based secure login system with role-based access control (Admin, Chef, Nutritionist, Customer, Meal Planner).
  - Email verification, account approval, and password reset functionality.

- **Recipe Management**
  - Add, update, delete, and retrieve recipes.
  - Store details like ingredients, preparation steps, and categories (e.g., vegan, keto, etc.).
  - Image upload functionality using Azure Blob Storage.

- **Meal Planning**
  - Create, update, and delete meal plans for different users.
  - Schedule meals for specific dates and times, ensuring dietary goals and preferences.

- **Shopping List Management**
  - Generate shopping lists automatically from meal plans.
  - Mark ingredients as purchased or not purchased.

- **Nutritional Tracking**
  - Add, update, and retrieve nutritional data for recipes.
  - View pending meals without nutritional data.

### Frontend Features
- **User Interfaces**
  - Login, signup, and profile management screens.
  - Dynamic dashboards tailored to user roles (Admin, Chef, Nutritionist, etc.).

- **Recipe and Meal Plan Management**
  - Interactive forms to create and update recipes and meal plans.
  - Displays recipes with step-by-step cooking instructions and images.

- **Shopping List Tracking**
  - View and update shopping lists in real-time.
  - Responsive design for mobile, tablet, and desktop users.

- **Visual Design**
  - Tailwind CSS-based styling with custom branding:
    - Green: `#8DC63F`
    - Text: `#494949`
    - Headings: `#191B19`

## Technology Stack
- **Backend:** ASP.NET Core with C# (.NET 8+)
- **Frontend:** Angular (v17+)
- **Database:** SQL Server 2022 or MongoDB 6.0
- **Authentication:** JWT
- **Styling:** Tailwind CSS

## Installation

### Prerequisites
1. .NET 8 or later.
2. Node.js and Angular CLI.
3. SQL Server or MongoDB instance.
4. Azure Blob Storage for image handling.

### Backend Setup
1. Clone the repository and navigate to the backend directory.
2. Update the connection strings in `appsettings.json`.
3. Run database migrations:
   ```bash
   dotnet ef database update
   ```
4. Start the backend server:
   ```bash
   dotnet run
   ```

### Frontend Setup
1. Navigate to the frontend directory.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   ng serve
   ```

## API Endpoints

### AuthController
- `/api/auth/signup`: User registration with email verification.
- `/api/auth/login`: Login with JWT token generation.
- `/api/auth/request-password-reset`: Password reset request.
- `/api/auth/reset-password`: Reset password using a token.

### RecipeController
- `/api/recipe`: Create, update, delete, and retrieve recipes.

### MealPlanController
- `/api/mealplan`: Manage meal plans with date-based scheduling.

### ShoppingListController
- `/api/shopping`: Generate and manage shopping lists.

### NutritionController
- `/api/nutrition`: Add and retrieve nutritional data for recipes.

### AdminController
- `/api/admin`: Manage user roles and approval statuses.

## Usage
1. Sign up and log in with your role (Admin, Chef, Nutritionist, etc.).
2. Manage recipes, meal plans, and shopping lists based on permissions.
3. Track nutritional data dynamically.
