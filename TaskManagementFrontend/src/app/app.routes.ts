import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { TaskManagementComponent } from './components/task-management/task-management.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'tasks', component: TaskManagementComponent },
  { path: '**', redirectTo: '/login' }
];
