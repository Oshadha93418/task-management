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

  // Handle HTTP errors
  handleHttpError(error: any): void {
    if (error.status === 404) {
      this.showError('Resource not found.', 5000);
    } else if (error.status === 500) {
      this.showError('Server error. Please try again later.', 5000);
    } else if (error.status === 0) {
      this.showError('Network error. Please check your connection.', 5000);
    } else {
      this.showError(error.error?.message || 'An unexpected error occurred.', 5000);
    }
  }
} 