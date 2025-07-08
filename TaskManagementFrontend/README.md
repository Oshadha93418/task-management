# Task Management Frontend

A modern, responsive Angular application for managing tasks with authentication and real-time updates.

## Features

### Authentication
- User registration and login
- Session management with localStorage
- Automatic redirect to login for unauthenticated users
- Secure logout functionality

### Task Management
- **Create Tasks**: Add new tasks with title, description, and completion status
- **Read Tasks**: View all tasks with detailed information
- **Update Tasks**: Edit existing tasks and toggle completion status
- **Delete Tasks**: Remove tasks with confirmation dialog

### Advanced Features
- **Search**: Real-time search through task titles and descriptions
- **Filtering**: Filter tasks by status (All, Pending, Completed)
- **Sorting**: Sort by title, creation date, or completion status
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile devices

### User Experience
- Modern, clean UI with smooth animations
- Real-time form validation
- Loading states and error handling
- Success/error message notifications
- Intuitive navigation and user feedback

## Technology Stack

- **Angular 19**: Latest version with standalone components
- **TypeScript**: Type-safe development
- **Reactive Forms**: Form handling and validation
- **CSS Grid & Flexbox**: Modern layout techniques
- **CSS Custom Properties**: Dynamic theming
- **HTTP Client**: API communication with backend

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── login/
│   │   │   ├── login.component.ts
│   │   │   ├── login.component.html
│   │   │   └── login.component.css
│   │   └── task-management/
│   │       ├── task-management.component.ts
│   │       ├── task-management.component.html
│   │       └── task-management.component.css
│   ├── models/
│   │   ├── task.ts
│   │   └── user.ts
│   ├── services/
│   │   ├── auth.service.ts
│   │   └── task.service.ts
│   ├── app.component.ts
│   ├── app.component.html
│   ├── app.routes.ts
│   └── app.config.ts
├── styles.css
└── main.ts
```

## Getting Started

### Prerequisites
- Node.js (v18 or higher)
- npm or yarn
- Angular CLI: `npm install -g @angular/cli`

### Installation

1. **Install dependencies**:
   ```bash
   npm install
   ```

2. **Start the development server**:
   ```bash
   ng serve
   ```

3. **Open your browser**:
   Navigate to `http://localhost:4200`

### Build for Production

```bash
ng build
```

The build artifacts will be stored in the `dist/` directory.

## API Integration

The frontend communicates with the .NET Web API backend:

- **Base URL**: `https://localhost:7000/api`
- **Authentication**: Query parameter-based authentication
- **Endpoints**:
  - `POST /users/login` - User authentication
  - `POST /users/register` - User registration
  - `GET /tasks` - Retrieve all tasks
  - `POST /tasks` - Create new task
  - `PUT /tasks/{id}` - Update task
  - `DELETE /tasks/{id}` - Delete task

## Key Features Explained

### Authentication Flow
1. Users can register with username and password
2. Login validates credentials against backend
3. User session is stored in localStorage
4. Automatic redirect to tasks page after login
5. Logout clears session and redirects to login

### Task Management
- **Form Validation**: Real-time validation with custom error messages
- **CRUD Operations**: Full create, read, update, delete functionality
- **Status Toggle**: One-click task completion toggle
- **Bulk Operations**: Efficient task list management

### Responsive Design
- **Desktop**: Side-by-side layout with form and list
- **Tablet**: Stacked layout with optimized spacing
- **Mobile**: Single-column layout with touch-friendly controls

### Performance Optimizations
- **Lazy Loading**: Components loaded on demand
- **Efficient Filtering**: Client-side filtering and sorting
- **Minimal Re-renders**: Optimized change detection
- **Smooth Animations**: CSS transitions and transforms

## Development

### Code Style
- **TypeScript**: Strict type checking enabled
- **ESLint**: Code quality and consistency
- **Prettier**: Automatic code formatting
- **Angular Style Guide**: Follows official Angular conventions

### Testing
```bash
# Run unit tests
ng test

# Run end-to-end tests
ng e2e
```

### Code Generation
```bash
# Generate new component
ng generate component components/component-name

# Generate new service
ng generate service services/service-name
```

## Deployment

### Build for Production
```bash
ng build --configuration production
```

### Environment Configuration
- Development: `http://localhost:7000/api`
- Production: Update API URL in services

## Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure backend allows requests from `http://localhost:4200`
2. **API Connection**: Verify backend is running on port 7000
3. **Authentication**: Check that user credentials are correct
4. **Build Errors**: Clear cache with `ng cache clean`

### Development Tips
- Use Angular DevTools for debugging
- Monitor network requests in browser DevTools
- Check console for TypeScript compilation errors
- Verify API responses match expected interfaces

## Contributing

1. Follow Angular coding standards
2. Write meaningful commit messages
3. Test changes thoroughly
4. Update documentation as needed

## License

This project is part of a task management application demonstration.
