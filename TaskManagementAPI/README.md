# Task Management API

A .NET Web API backend for a task management application with user authentication and CRUD operations for tasks, built following SOLID principles with comprehensive error handling and validation.

## Architecture & SOLID Principles

This application is built following SOLID principles:

### 1. Single Responsibility Principle (SRP)
- **Controllers**: Handle HTTP requests and responses only
- **Services**: Contain business logic
- **Repositories**: Handle data access operations
- **Models**: Represent data structures
- **Exceptions**: Handle specific error scenarios
- **Middleware**: Handle cross-cutting concerns like logging and validation

### 2. Open/Closed Principle (OCP)
- Interfaces allow for extension without modification
- New implementations can be added without changing existing code
- Custom exceptions can be extended for new error types
- Middleware and filters can be extended for new validation rules

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
- **Comprehensive Error Handling**: Global exception handling with consistent error responses
- **Input Validation**: Multi-layer validation with sanitization
- **Request Logging**: Structured logging for all requests and errors
- **Security**: Input sanitization and validation to prevent injection attacks

## Error Handling & Validation

### Global Exception Handling
- **Middleware**: Custom middleware for request validation and error handling
- **Global Exception Handler**: Catches unhandled exceptions and returns consistent error responses
- **Structured Logging**: All errors are logged with context and request information

### Input Validation
- **Data Annotations**: Comprehensive validation attributes on all models
- **Business Logic Validation**: Additional validation in service layer
- **Input Sanitization**: Automatic sanitization of string inputs
- **Request Size Limits**: 1MB limit on request size
- **Content-Type Validation**: Ensures proper JSON content type for POST/PUT requests

### Error Response Format
All error responses follow a consistent format:
```json
{
  "message": "Error description",
  "statusCode": 400,
  "timestamp": "2024-01-01T00:00:00Z",
  "requestId": "unique-request-id",
  "details": ["Additional error details"]
}
```

### Validation Rules

#### Task Validation
- **Title**: Required, 1-100 characters, alphanumeric with basic punctuation
- **Description**: Optional, max 500 characters, alphanumeric with basic punctuation
- **ID**: Must be positive integer for updates/deletes

#### User Validation
- **Username**: Required, 3-50 characters, alphanumeric with underscores and hyphens only
- **Password**: Required, 6-100 characters, no whitespace allowed

#### Authentication Validation
- **Login/Register**: All user validation rules apply
- **Input Sanitization**: Automatic trimming and whitespace normalization

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
│   ├── TaskManagementException.cs # Custom exception classes
│   └── InvalidCredentialsException.cs # Authentication exceptions
├── Filters/
│   └── RequestValidationFilter.cs # Global request validation
├── Interfaces/
│   ├── IAuthService.cs            # Authentication interface
│   ├── ITaskRepository.cs         # Task data access interface
│   ├── ITaskService.cs            # Task business logic interface
│   └── IUserRepository.cs         # User data access interface
├── Middleware/
│   └── RequestValidationMiddleware.cs # Request validation and logging
├── Models/
│   ├── Task.cs                    # Task entity with validation
│   ├── User.cs                    # User entity with validation
│   ├── AuthModels.cs              # Authentication DTOs
│   ├── TaskDtos.cs                # Task operation DTOs
│   └── ErrorResponse.cs           # Consistent error response format
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

- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get specific task
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

## Default Users

- **Username**: admin, **Password**: password123
- **Username**: user, **Password**: password123

## Request/Response Examples

### Register User
```json
POST /api/users/register
Content-Type: application/json

{
  "username": "newuser",
  "password": "password123"
}
```

**Success Response:**
```json
{
  "message": "Registration successful",
  "user": {
    "id": 3,
    "username": "newuser",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

**Validation Error Response:**
```json
{
  "message": "Validation failed",
  "statusCode": 400,
  "timestamp": "2024-01-01T00:00:00Z",
  "requestId": "unique-request-id",
  "details": [
    "Username must be between 3 and 50 characters",
    "Username can only contain letters, numbers, underscores, and hyphens"
  ]
}
```

### Create Task
```json
POST /api/tasks
Content-Type: application/json

{
  "title": "New Task",
  "description": "Task description"
}
```

**Success Response:**
```json
{
  "id": 1,
  "title": "New Task",
  "description": "Task description",
  "isCompleted": false,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

## Error Handling

The application implements comprehensive error handling:

### Custom Exceptions
- `TaskNotFoundException`: When a task is not found
- `UserNotFoundException`: When a user is not found
- `UserAlreadyExistsException`: When trying to create a duplicate user
- `ValidationException`: When input validation fails
- `InvalidCredentialsException`: When authentication fails

### HTTP Status Codes
- `200 OK`: Successful operation
- `201 Created`: Resource created successfully
- `400 Bad Request`: Validation errors or invalid input
- `401 Unauthorized`: Authentication failed
- `404 Not Found`: Resource not found
- `409 Conflict`: Resource already exists
- `413 Payload Too Large`: Request too large
- `500 Internal Server Error`: Unexpected server error

### Error Response Features
- **Consistent Format**: All errors follow the same structure
- **Request Tracking**: Each error includes a unique request ID
- **Timestamp**: All errors include UTC timestamp
- **Detailed Messages**: Clear, user-friendly error messages
- **Validation Details**: Specific validation error details when applicable

## Security Features

### Input Validation
- **SQL Injection Prevention**: Parameterized queries and input validation
- **XSS Prevention**: Input sanitization and output encoding
- **Request Size Limits**: Prevents large payload attacks
- **Content-Type Validation**: Ensures proper request format

### Authentication
- **Input Sanitization**: Automatic trimming and normalization
- **Password Validation**: No whitespace allowed in passwords
- **Username Validation**: Restricted character set for usernames

## Benefits of Enhanced Error Handling

1. **User Experience**: Clear, consistent error messages
2. **Debugging**: Detailed logging and request tracking
3. **Security**: Input validation and sanitization
4. **Maintainability**: Centralized error handling logic
5. **Monitoring**: Structured logging for operational insights
6. **Compliance**: Consistent error response format for API consumers

## Benefits of SOLID Implementation

1. **Maintainability**: Code is easier to understand and modify
2. **Testability**: Each component can be tested in isolation
3. **Extensibility**: New features can be added without changing existing code
4. **Reusability**: Components can be reused in different contexts
5. **Flexibility**: Dependencies can be easily swapped or mocked 