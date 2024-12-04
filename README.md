# RAMPS - Recipe and Meal Management Platform Software



# Backend -  RecipeMeal API

The **RecipeMeal API** is a backend solution for managing recipes, meal plans, and user roles using .NET 8. It includes authentication using JWT and role-based access control (RBAC).

## Features
- User registration and login with JWT authentication.
- Role-based access control (e.g., Admin, User).
- Secure database connection using Azure SQL Server.
- Endpoints for managing users and roles.

---

## Prerequisites
1. **.NET 8 SDK** installed on your system.
2. A **SQL Server** instance (e.g., Azure SQL).
3. A valid configuration for JWT in your `appsettings.json`.

---

## Setup and Configuration

1. Clone the repository:
    ```bash
    git clone <your-repo-url>
    cd RecipeMeal
    ```

2. Add the `appsettings.json` file in the `RecipeMeal.API` project folder with the following configuration:

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=<your-server>.database.windows.net;Database=<your-database>;User Id=<your-username>;Password=<your-password>;TrustServerCertificate=False;Encrypt=True;"
    },
    "Jwt": {
        "Issuer": "RecipeMealAPI",
        "Audience": "RecipeMealAPI",
        "Secret": "<your-jwt-secret>"
    }
}
```

> Replace `<your-server>`, `<your-database>`, `<your-username>`, `<your-password>`, and `<your-jwt-secret>` with your actual configuration details. Do not hardcode sensitive information directly into the source code.

3. Apply database migrations:
    ```bash
    cd RecipeMeal.Infrastructure
    dotnet ef migrations add InitialCreate --startup-project ../RecipeMeal.API --project .
    ```

4. Update the database:
    ```bash
    dotnet ef database update --startup-project ../RecipeMeal.API --project .
    ```

5. Run the API:
    ```bash
    dotnet run --project RecipeMeal.API
    ```

6. Open Swagger to test the API endpoints:
    ```
    http://localhost:<your-port>/swagger
    ```

---

## Testing Endpoints
- Use tools like **Postman** or Swagger UI to test the API.
- Endpoints requiring authentication must include a valid `Bearer` token in the request header.

---

## Security Considerations
- Keep your `appsettings.json` file secure and never commit sensitive details to version control.
- Use environment variables or secret managers for production configuration.

---
