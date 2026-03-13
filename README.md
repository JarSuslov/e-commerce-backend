# ECommerce API

REST API for an online store built with ASP.NET Core + PostgreSQL.

## Tech Stack

- .NET 10 / ASP.NET Core Web API
- PostgreSQL 16 + Entity Framework Core (Code First)
- JWT Authentication (access token)
- FluentValidation
- Swagger (Swashbuckle)
- Docker + docker-compose
- Rate Limiting (Microsoft.AspNetCore.RateLimiting)

## Quick Start (Docker)

```bash
docker-compose up --build
```

API available at: **http://localhost:8080**  
Swagger UI: **http://localhost:8080**

## Test Accounts (Seed Data)

| Role     | Email              | Password      |
|----------|--------------------|---------------|
| Admin    | admin@store.com    | Admin123!     |
| Customer | customer@store.com | Customer123!  |

## How to Test via Swagger

1. Open http://localhost:8080 in a browser
2. Execute `POST /api/auth/login` with body:
   ```json
   {
     "email": "admin@store.com",
     "password": "Admin123!"
   }
   ```
3. Copy the `token` value from the response
4. Click the **Authorize** button (🔒) at the top of the page
5. Paste the token (without the `Bearer` prefix)
6. Click **Authorize** — all protected endpoints are now accessible

## Endpoints

### Authentication
| Method | URL                  | Description              |
|--------|----------------------|--------------------------|
| POST   | /api/auth/register   | Register a new customer  |
| POST   | /api/auth/login      | Login (get JWT token)    |

### Categories (Admin for CUD, GET is public)
| Method | URL                    | Description          |
|--------|------------------------|----------------------|
| GET    | /api/categories        | List all categories  |
| GET    | /api/categories/{id}   | Get category by ID   |
| POST   | /api/categories        | Create category      |
| PUT    | /api/categories/{id}   | Update category      |
| DELETE | /api/categories/{id}   | Delete category      |

### Products (Admin for CUD, GET is public)
| Method | URL                  | Description                        |
|--------|----------------------|------------------------------------|
| GET    | /api/products        | List with filters + pagination     |
| GET    | /api/products/{id}   | Get product by ID                  |
| POST   | /api/products        | Create product                     |
| PUT    | /api/products/{id}   | Update product                     |
| DELETE | /api/products/{id}   | Delete product                     |

Query parameters for GET /api/products:
- `search` — search by name
- `categoryId` — filter by category
- `page` — page number (default: 1)
- `pageSize` — page size (default: 10, max: 50)

### Cart (requires authentication)
| Method | URL                        | Description             |
|--------|----------------------------|-------------------------|
| GET    | /api/cart                  | View cart               |
| POST   | /api/cart                  | Add product to cart     |
| DELETE | /api/cart/{productId}      | Remove product from cart|

### Orders (requires authentication)
| Method | URL                  | Description              |
|--------|----------------------|--------------------------|
| POST   | /api/orders          | Checkout (create order)  |
| GET    | /api/orders          | Get my orders            |
| GET    | /api/orders/{id}     | Get order by ID          |

### Admin — All Orders
| Method | URL                  | Description              |
|--------|----------------------|--------------------------|
| GET    | /api/admin/orders    | All orders (Admin only)  |

## Local Development (without Docker)

1. Install PostgreSQL and create database `ecommerce_db`
2. Update the connection string in `appsettings.Development.json`
3. Run:
```bash
cd ECommerce.API
dotnet ef migrations add InitialCreate
dotnet run
```

## Project Structure

```
ECommerce.API/
├── Controllers/          # API controllers
├── Common/
│   ├── Exceptions/       # Custom exceptions
│   ├── Middleware/        # ExceptionHandlingMiddleware
│   └── Extensions/       # Extension methods
├── Data/                 # DbContext + Seed
├── DTOs/                 # Request/Response models
│   ├── Auth/
│   ├── Cart/
│   ├── Category/
│   ├── Order/
│   └── Product/
├── Entities/             # EF Core entities
├── Services/             # Business logic
│   └── Interfaces/
├── Validators/           # FluentValidation
├── Program.cs            # Entry point + configuration
├── appsettings.json
└── appsettings.Development.json
```
