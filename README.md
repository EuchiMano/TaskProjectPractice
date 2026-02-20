# TasksProject - ASP.NET Core API with JWT Authentication

A practice project demonstrating JWT authentication, role-based authorization, and custom middleware in ASP.NET Core. Built with .NET 8, Entity Framework Core, and SQL Server.

## Features

- **JWT Authentication**: Secure token-based authentication.
- **Role-Based Authorization**: Admin and User roles with different permissions.
- **Custom Middleware**: Request logging for authenticated users.
- **CRUD API**: Manage tasks with full REST operations.
- **Swagger UI**: Interactive API documentation.

## Tech Stack

- **Backend**: ASP.NET Core 8 Web API
- **Database**: SQL Server (via Entity Framework Core)
- **Authentication**: JWT Bearer Tokens with ASP.NET Core Identity
- **Logging**: Built-in ILogger with custom middleware

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (local or remote, e.g., via Docker or local instance)
- Git

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/TasksProject.git
   cd TasksProject
   ```

2. **Restore packages**:
   ```bash
   dotnet restore
   ```

3. **Update connection string** (in `appsettings.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your-server;Database=TasksDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. **Run migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

## Authentication and Authorization Flow

### Overview
This project uses JWT (JSON Web Tokens) for stateless authentication and ASP.NET Core Identity for user management. Roles (Admin/User) control access to endpoints.

### Step-by-Step Flow

1. **User Registration**:
   - Endpoint: `POST /api/auth/register`
   - Body: `{ "username": "testuser", "email": "test@example.com", "password": "Password123!", "role": "User" }`
   - Creates a new user in the database with the specified role (default: "User").
   - Password is hashed using Identity's secure hashing.

2. **User Login**:
   - Endpoint: `POST /api/auth/login`
   - Body: `{ "username": "testuser", "password": "Password123!" }`
   - Validates credentials against the database.
   - If valid, generates a JWT token containing:
     - User claims (username, roles).
     - Issuer, Audience, Expiration (3 hours).
     - Signed with a secret key.
   - Returns: `{ "token": "eyJ...", "expiration": "2026-02-20T..." }`

3. **Authenticated Requests**:
   - Include the token in the `Authorization` header: `Bearer <token>`
   - Middleware validates the token on each request:
     - Checks signature, issuer, audience, and expiration.
     - Populates `HttpContext.User` with claims.
   - Custom `RequestLoggingMiddleware` logs authenticated requests (user, method, path, status).

4. **Authorization**:
   - All `/api/tasks` endpoints require authentication (`[Authorize]`).
   - Delete endpoint requires "Admin" role (`[Authorize(Roles = "Admin")]`).
   - If unauthorized, returns 401/403.

5. **Middleware Pipeline**:
   ```
   Request → HTTPS Redirection → Authentication (JWT) → Request Logging → Authorization → Controllers → Response
   ```

### Example API Calls

- **Register Admin**: `POST /api/auth/register` with role "Admin"
- **Login**: `POST /api/auth/login`
- **Get Tasks**: `GET /api/tasks` (with Bearer token)
- **Create Task**: `POST /api/tasks` (with Bearer token)
- **Delete Task**: `DELETE /api/tasks/{id}` (Admin only)

## Project Structure

```
TasksProject/
├── Controllers/
│   ├── AuthController.cs      # Registration/Login
│   └── TasksController.cs     # CRUD operations
├── Data/
│   └── TasksDbContext.cs      # EF Core context
├── Middleware/
│   └── RequestLoggingMiddleware.cs  # Custom logging
├── Models/
│   └── TaskItem.cs            # Task model
├── appsettings.json           # Config (JWT, DB)
├── Program.cs                 # App startup
└── TasksProject.csproj        # Project file
```

## Postman Collection

Import `TasksProject.postman_collection.json` into Postman for testing.

### Collection Structure
- **Auth Folder**:
  - Register User
  - Register Admin
  - Login
- **Tasks Folder**:
  - Get All Tasks
  - Get Task by ID
  - Create Task
  - Delete Task (Admin)

Set environment variables: `baseUrl` (e.g., `https://localhost:5001`), `token` (from login response).

## Learning Outcomes

- JWT token generation/validation
- ASP.NET Core Identity for users/roles
- Custom middleware implementation
- Role-based authorization
- EF Core migrations and database setup
- API security best practices

## Contributing

Feel free to fork and enhance! Add features like refresh tokens, email confirmation, or unit tests.

## License

MIT License
