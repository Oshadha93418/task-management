import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/task';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'http://localhost:5263/api';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  private getHeaders(): HttpHeaders {
    const currentUser = this.authService.getCurrentUser();
    const headers = new HttpHeaders();
    
    if (currentUser) {
      return headers.set('X-User-Id', currentUser.id.toString());
    }
    
    return headers;
  }

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.apiUrl}/tasks`, { headers: this.getHeaders() });
  }

  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/tasks/${id}`, { headers: this.getHeaders() });
  }

  createTask(task: Task): Observable<Task> {
    return this.http.post<Task>(`${this.apiUrl}/tasks`, task, { headers: this.getHeaders() });
  }

  updateTask(id: number, task: Task): Observable<any> {
    return this.http.put(`${this.apiUrl}/tasks/${id}`, task, { headers: this.getHeaders() });
  }

  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/tasks/${id}`, { headers: this.getHeaders() });
  }

  toggleTaskCompletion(task: Task): Observable<any> {
    const updatedTask = { ...task, isCompleted: !task.isCompleted };
    return this.updateTask(task.id!, updatedTask);
  }
} 