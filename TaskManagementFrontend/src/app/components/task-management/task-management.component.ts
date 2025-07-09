import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { Task } from '../../models/task';
import { TaskService } from '../../services/task.service';
import { AuthService } from '../../services/auth.service';
import { ErrorService } from '../../services/error.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-management',
  templateUrl: './task-management.component.html',
  styleUrls: ['./task-management.component.css'],
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  standalone: true
})
export class TaskManagementComponent implements OnInit {
  tasks: Task[] = [];
  filteredTasks: Task[] = [];
  taskForm: FormGroup;
  editingTask: Task | null = null;
  isLoading = false;

  // Filter and sort properties
  filterStatus: 'all' | 'completed' | 'pending' = 'all';
  sortBy: 'title' | 'createdAt' | 'isCompleted' = 'createdAt';
  sortOrder: 'asc' | 'desc' = 'desc';
  searchTerm = '';

  constructor(
    private taskService: TaskService,
    public authService: AuthService,
    private errorService: ErrorService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      isCompleted: [false]
    });
  }

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadTasks();
  }

  loadTasks(): void {
    this.isLoading = true;

    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.applyFiltersAndSort();
        this.isLoading = false;
      },
      error: (error) => {
        this.errorService.showError('Failed to load tasks. Please try again.');
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      this.isLoading = true;

      const currentUser = this.authService.getCurrentUser();
      if (!currentUser) {
        this.errorService.showError('User not authenticated. Please login again.');
        this.router.navigate(['/login']);
        return;
      }

      const taskData: Task = {
        ...this.taskForm.value,
        userId: currentUser.id
      };

      if (this.editingTask) {
        // Update existing task
        const updatedTask = { ...this.editingTask, ...taskData };
        this.taskService.updateTask(this.editingTask.id!, updatedTask).subscribe({
          next: () => {
            this.errorService.showInfo('Task updated successfully!');
            this.resetForm();
            this.loadTasks();
          },
          error: (error) => {
            this.errorService.showError(error.error || 'Failed to update task.');
            this.isLoading = false;
          }
        });
      } else {
        // Create new task
        this.taskService.createTask(taskData).subscribe({
          next: () => {
            this.errorService.showInfo('Task created successfully!');
            this.resetForm();
            this.loadTasks();
          },
          error: (error) => {
            this.errorService.showError(error.error || 'Failed to create task.');
            this.isLoading = false;
          }
        });
      }
    }
  }

  editTask(task: Task): void {
    this.editingTask = task;
    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      isCompleted: task.isCompleted
    });
  }

  cancelEdit(): void {
    this.resetForm();
  }

  deleteTask(taskId: number): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.isLoading = true;

      this.taskService.deleteTask(taskId).subscribe({
        next: () => {
          this.errorService.showInfo('Task deleted successfully!');
          this.loadTasks();
        },
        error: (error) => {
          this.errorService.showError('Failed to delete task.');
          this.isLoading = false;
        }
      });
    }
  }

  toggleTaskCompletion(task: Task): void {
    this.taskService.toggleTaskCompletion(task).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (error) => {
        this.errorService.showError('Failed to update task status.');
      }
    });
  }

  resetForm(): void {
    this.taskForm.reset({ isCompleted: false });
    this.editingTask = null;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Filter and sort methods
  applyFiltersAndSort(): void {
    let filtered = [...this.tasks];

    // Apply status filter
    if (this.filterStatus !== 'all') {
      filtered = filtered.filter(task => 
        this.filterStatus === 'completed' ? task.isCompleted : !task.isCompleted
      );
    }

    // Apply search filter
    if (this.searchTerm.trim()) {
      const search = this.searchTerm.toLowerCase();
      filtered = filtered.filter(task =>
        task.title.toLowerCase().includes(search) ||
        (task.description && task.description.toLowerCase().includes(search))
      );
    }

    // Apply sorting
    filtered.sort((a, b) => {
      let aValue: any, bValue: any;

      switch (this.sortBy) {
        case 'title':
          aValue = a.title.toLowerCase();
          bValue = b.title.toLowerCase();
          break;
        case 'createdAt':
          aValue = new Date(a.createdAt!);
          bValue = new Date(b.createdAt!);
          break;
        case 'isCompleted':
          aValue = a.isCompleted;
          bValue = b.isCompleted;
          break;
        default:
          return 0;
      }

      if (aValue < bValue) return this.sortOrder === 'asc' ? -1 : 1;
      if (aValue > bValue) return this.sortOrder === 'asc' ? 1 : -1;
      return 0;
    });

    this.filteredTasks = filtered;
  }

  onFilterChange(): void {
    this.applyFiltersAndSort();
  }

  onSortChange(): void {
    this.applyFiltersAndSort();
  }

  onSearchChange(): void {
    this.applyFiltersAndSort();
  }
}
