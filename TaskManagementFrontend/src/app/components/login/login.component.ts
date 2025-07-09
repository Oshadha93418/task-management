import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ErrorService } from '../../services/error.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [ReactiveFormsModule, CommonModule],
  standalone: true
})
export class LoginComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  isLoginMode = true;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    // Clear any existing error messages when switching modes
    this.errorService.clearErrors();
  }

  onSubmit(): void {
    if (this.isLoginMode) {
      this.login();
    } else {
      this.register();
    }
  }

  private handleAuthError(error: any, action: string): void {
    // If there's a specific error message from the server, show it
    if (error.error?.message) {
      this.errorService.showError(error.error.message);
      return;
    }
    
    // Fallback to generic error handling
    this.errorService.handleAuthError(error, action as 'login' | 'register');
  }

  login(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;

      this.authService.login(this.loginForm.value).subscribe({
        next: () => {
          this.router.navigate(['/tasks']);
        },
        error: (error) => {
          this.handleAuthError(error, 'login');
          this.isLoading = false;
        }
      });
    }
  }

  register(): void {
    if (this.registerForm.valid) {
      this.isLoading = true;

      const { confirmPassword, ...registerData } = this.registerForm.value;
      
      this.authService.register(registerData).subscribe({
        next: () => {
          this.errorService.showInfo('Registration successful! Please log in with your new account.');
          this.isLoginMode = true;
          this.isLoading = false;
          // Clear the register form
          this.registerForm.reset();
        },
        error: (error) => {
          this.handleAuthError(error, 'register');
          this.isLoading = false;
        }
      });
    }
  }
}
