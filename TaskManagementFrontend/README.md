# Task Management Frontend

A modern Angular application for managing tasks with user-specific access control.

## Features

- **User Authentication**: Secure login and registration system
- **User-Specific Tasks**: Each user can only view and manage their own tasks
- **Task Management**: Create, read, update, and delete tasks
- **Task Filtering**: Filter tasks by status (all, pending, completed)
- **Task Sorting**: Sort tasks by title, creation date, or completion status
- **Search**: Search tasks by title or description
- **Responsive Design**: Works on desktop and mobile devices

## Authentication System

The application uses a simple header-based authentication system:

- Users must login to access the task management features
- The `X-User-Id` header is automatically included in all API requests
- If authentication fails (401 error), users are automatically redirected to login
- User sessions are stored in localStorage

## API Integration

The frontend communicates with the Task Management API:

- **Base URL**: `http://localhost:5263/api`
- **Authentication**: Uses `X-User-Id` header for all requests
- **Error Handling**: Comprehensive error handling with user-friendly messages

## Development

### Prerequisites

- Node.js (v16 or higher)
- npm or yarn

### Installation

```bash
npm install
```

### Development Server

```bash
npm start
```

The application will be available at `http://localhost:4200`.

### Build

```bash
npm run build
```

### Testing

```bash
npm test
```

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── login/              # Login component
│   │   ├── task-management/    # Main task management component
│   │   └── error-notifications/ # Error display component
│   ├── services/
│   │   ├── auth.service.ts     # Authentication service
│   │   ├── task.service.ts     # Task management service
│   │   └── error.service.ts    # Error handling service
│   ├── models/
│   │   ├── user.ts            # User-related interfaces
│   │   └── task.ts            # Task interface
│   └── interceptors/
│       └── auth.interceptor.ts # HTTP authentication interceptor
```

## User Flow

1. **Login**: Users must login with their credentials
2. **Task Management**: Users can only see and manage their own tasks
3. **Authentication**: All API requests include the user's ID automatically
4. **Session Management**: User sessions persist until logout

## Security Features

- **User Isolation**: Each user can only access their own tasks
- **Automatic Logout**: Users are logged out on authentication errors
- **Input Validation**: Client-side validation for all forms
- **Error Handling**: Comprehensive error handling with user feedback

## API Endpoints Used

- `POST /api/users/login` - User authentication
- `POST /api/users/register` - User registration
- `GET /api/tasks` - Get user's tasks
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

All task endpoints require the `X-User-Id` header for authentication.
