import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ErrorMessage {
  message: string;
  type: 'error' | 'warning' | 'info';
  duration?: number;
  id?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  private errorsSubject = new BehaviorSubject<ErrorMessage[]>([]);
  public errors$ = this.errorsSubject.asObservable();

  constructor() { }

  showError(message: string, duration: number = 5000): void {
    const error: ErrorMessage = {
      message,
      type: 'error',
      duration,
      id: Date.now().toString()
    };
    this.addError(error);
  }

  showWarning(message: string, duration: number = 5000): void {
    const warning: ErrorMessage = {
      message,
      type: 'warning',
      duration,
      id: Date.now().toString()
    };
    this.addError(warning);
  }

  showInfo(message: string, duration: number = 3000): void {
    const info: ErrorMessage = {
      message,
      type: 'info',
      duration,
      id: Date.now().toString()
    };
    this.addError(info);
  }

  private addError(error: ErrorMessage): void {
    const currentErrors = this.errorsSubject.value;
    this.errorsSubject.next([...currentErrors, error]);

    if (error.duration && error.duration > 0) {
      setTimeout(() => {
        this.removeError(error.id!);
      }, error.duration);
    }
  }

  removeError(id: string): void {
    const currentErrors = this.errorsSubject.value;
    const filteredErrors = currentErrors.filter(error => error.id !== id);
    this.errorsSubject.next(filteredErrors);
  }

  clearErrors(): void {
    this.errorsSubject.next([]);
  }

  // Handle HTTP errors with more specific messages
  handleHttpError(error: any): void {
    if (error.status === 0) {
      this.showError('Network error. Please check your internet connection and try again.', 5000);
    } else if (error.status === 400) {
      this.showError(error.error?.message || 'Invalid request. Please check your input and try again.', 5000);
    } else if (error.status === 401) {
      this.showError('Authentication failed. Please check your credentials and try again.', 5000);
    } else if (error.status === 403) {
      this.showError('Access denied. You do not have permission to perform this action.', 5000);
    } else if (error.status === 404) {
      this.showError('Resource not found. Please check the URL and try again.', 5000);
    } else if (error.status === 409) {
      this.showError('Conflict detected. The resource already exists or has been modified.', 5000);
    } else if (error.status === 422) {
      this.showError(error.error?.message || 'Validation error. Please check your input and try again.', 5000);
    } else if (error.status === 429) {
      this.showError('Too many requests. Please wait a moment and try again.', 5000);
    } else if (error.status >= 500) {
      this.showError('Server error. Please try again later or contact support if the problem persists.', 5000);
    } else {
      this.showError(error.error?.message || 'An unexpected error occurred. Please try again.', 5000);
    }
  }

  // Handle authentication-specific errors
  handleAuthError(error: any, action: 'login' | 'register'): void {
    // Check for server error message first
    const serverMessage = error.error?.message || error.error?.error || error.message;
    
    if (serverMessage) {
      this.showError(serverMessage, 5000);
      return;
    }
    
    if (error.status === 0) {
      this.showError('Network error. Please check your internet connection and try again.', 5000);
    } else if (error.status === 401) {
      if (action === 'login') {
        this.showError('Invalid username or password. Please check your credentials and try again.', 5000);
      } else {
        this.showError('Authentication failed. Please try again.', 5000);
      }
    } else if (error.status === 400) {
      if (action === 'register') {
        if (error.error?.message?.toLowerCase().includes('username')) {
          this.showError('Username already exists. Please choose a different username.', 5000);
        } else if (error.error?.message?.toLowerCase().includes('password')) {
          this.showError('Password does not meet requirements. Please use a stronger password.', 5000);
        } else {
          this.showError(error.error?.message || 'Invalid registration data. Please check your information.', 5000);
        }
      } else {
        this.showError(error.error?.message || 'Invalid login data. Please check your information.', 5000);
      }
    } else if (error.status === 422) {
      this.showError(error.error?.message || 'Validation error. Please check your input.', 5000);
    } else {
      this.handleHttpError(error);
    }
  }
} 