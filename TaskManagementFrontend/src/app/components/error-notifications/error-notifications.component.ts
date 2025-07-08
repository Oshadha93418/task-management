import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface ErrorNotification {
  id: string;
  message: string;
  type: 'error' | 'warning' | 'info';
}

@Component({
  selector: 'app-error-notifications',
  imports: [CommonModule],
  templateUrl: './error-notifications.component.html',
  styleUrl: './error-notifications.component.css'
})
export class ErrorNotificationsComponent {
  errors: ErrorNotification[] = [];

  getErrorClass(type: string): string {
    switch (type) {
      case 'error':
        return 'bg-red-50 border-red-200 text-red-800';
      case 'warning':
        return 'bg-yellow-50 border-yellow-200 text-yellow-800';
      case 'info':
        return 'bg-blue-50 border-blue-200 text-blue-800';
      default:
        return 'bg-gray-50 border-gray-200 text-gray-800';
    }
  }

  getIconClass(type: string): string {
    switch (type) {
      case 'error':
        return 'text-red-400';
      case 'warning':
        return 'text-yellow-400';
      case 'info':
        return 'text-blue-400';
      default:
        return 'text-gray-400';
    }
  }

  removeError(id: string): void {
    this.errors = this.errors.filter(error => error.id !== id);
  }
}
