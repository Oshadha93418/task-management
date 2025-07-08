# Task Management API

A .NET Web API backend for a task management application with user authentication and CRUD operations for tasks, built following SOLID principles.

## Architecture & SOLID Principles

This application is built following SOLID principles:

### 1. Single Responsibility Principle (SRP)
- **Controllers**: Handle HTTP requests and responses only
- **Services**: Contain business logic
- **Repositories**: Handle data access operations
- **Models**: Represent data structures
- **Exceptions**: Handle specific error scenarios

### 2. Open/Closed Principle (OCP)
- Interfaces allow for extension without modification
- New implementations can be added without changing existing code
- Custom exceptions can be extended for new error types

### 3. Liskov Substitution Principle (LSP)
- All implementations can be substituted for their interfaces
- Repository implementations are interchangeable
- Service implementations follow the same contract

### 4. Interface Segregation Principle (ISP)
- `ITaskRepository`: Only task-related data operations
- `IUserRepository`: Only user-related data operations
- `IAuthService`: Only authentication operations
- `ITaskService`: Only task business logic

### 5. Dependency Inversion Principle (DIP)
- High-level modules (Controllers) depend on abstractions (Interfaces)
- Low-level modules (Repositories) implement abstractions
- Dependency injection is used throughout the application

## Features

- **User Authentication**: Simple username/password authentication
- **Task Management**: Full CRUD operations for tasks
- **Database**: SQL Server with Entity Framework Core
- **RESTful API**: Standard REST endpoints
- **CORS Support**: Configured for frontend integration
- **SOLID Architecture**: Clean separation of concerns
- **Custom Exceptions**: Proper error handling
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic separation

## Prerequisites

- .NET 9.0 SDK
- SQL Server or SQL Server Express
- Entity Framework Core tools

## Setup

### 1. Database Setup

Run the SQL script `DatabaseSetup.sql` in your SQL Server instance to create the database and initial data.

### 2. Connection String

Update the connection string in `appsettings.json` to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManagementDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 3. Run Migrations

```bash
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

The API will be available at `https://localhost:7000` (or the port shown in the console).

## Project Structure

```
TaskManagementAPI/
├── Controllers/
│   ├── BaseController.cs          # Common authentication logic
│   ├── TasksController.cs         # Task CRUD operations
│   └── UsersController.cs         # User authentication
├── Data/
│   └── ApplicationDbContext.cs    # Entity Framework context
├── Exceptions/
│   └── TaskManagementException.cs # Custom exception classes
├── Interfaces/
│   ├── IAuthService.cs            # Authentication interface
│   ├── ITaskRepository.cs         # Task data access interface
│   ├── ITaskService.cs            # Task business logic interface
│   └── IUserRepository.cs         # User data access interface
├── Models/
│   ├── Task.cs                    # Task entity
│   └── User.cs                    # User entity
├── Repositories/
│   ├── TaskRepository.cs          # Task data access implementation
│   └── UserRepository.cs          # User data access implementation
├── Services/
│   ├── AuthService.cs             # Authentication business logic
│   └── TaskService.cs             # Task business logic
├── DatabaseSetup.sql              # Database initialization script
└── README.md                      # This file
```

## API Endpoints

### Authentication

- `POST /api/users/register` - Register a new user
- `POST /api/users/login` - Login with username/password

### Tasks

- `GET /api/tasks?username={username}&password={password}` - Get all tasks
- `GET /api/tasks/{id}?username={username}&password={password}` - Get specific task
- `POST /api/tasks?username={username}&password={password}` - Create new task
- `PUT /api/tasks/{id}?username={username}&password={password}` - Update task
- `DELETE /api/tasks/{id}?username={username}&password={password}` - Delete task

## Default Users

- **Username**: admin, **Password**: password123
- **Username**: user, **Password**: password123

## Request/Response Examples

### Register User
```json
POST /api/users/register
{
  "username": "newuser",
  "password": "password123"
}
```

### Login
```json
POST /api/users/login
{
  "username": "admin",
  "password": "password123"
}
```

### Create Task
```json
POST /api/tasks?username=admin&password=password123
{
  "title": "New Task",
  "description": "Task description",
  "isCompleted": false
}
```

### Get Tasks
```
GET /api/tasks?username=admin&password=password123
```

## Error Handling

The application uses custom exceptions for better error handling:

- `TaskNotFoundException`: When a task is not found
- `UserNotFoundException`: When a user is not found
- `UserAlreadyExistsException`: When trying to create a duplicate user
- `ValidationException`: When input validation fails

## Benefits of SOLID Implementation

1. **Maintainability**: Code is easier to understand and modify
2. **Testability**: Each component can be tested in isolation
3. **Extensibility**: New features can be added without changing existing code
4. **Reusability**: Components can be reused in different contexts
5. **Flexibility**: Dependencies can be easily swapped or mocked 