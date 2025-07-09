using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;
using Microsoft.Extensions.Logging;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _taskRepository.GetAllAsync();
                return tasks.Select(MapToResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all tasks");
                throw;
            }
        }

        public async Task<TaskResponse?> GetTaskByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("Task ID must be a positive integer");
                }

                var taskEntity = await _taskRepository.GetByIdAsync(id);
                return taskEntity != null ? MapToResponse(taskEntity) : null;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task with ID {TaskId}", id);
                throw;
            }
        }

        public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
        {
            try
            {
                // Sanitize input
                request.SanitizeInput();

                // Additional business logic validation
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    throw new ValidationException("Task title is required");
                }

                if (request.Title.Length > 100)
                {
                    throw new ValidationException("Task title cannot exceed 100 characters");
                }

                if (request.Description?.Length > 500)
                {
                    throw new ValidationException("Task description cannot exceed 500 characters");
                }

                var taskEntity = new Models.Task
                {
                    Title = request.Title,
                    Description = request.Description,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdTask = await _taskRepository.CreateAsync(taskEntity);
                _logger.LogInformation("Task created with ID {TaskId}", createdTask.Id);
                
                return MapToResponse(createdTask);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                throw;
            }
        }

        public async Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("Task ID must be a positive integer");
                }

                // Sanitize input
                request.SanitizeInput();

                // Additional business logic validation
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    throw new ValidationException("Task title is required");
                }

                if (request.Title.Length > 100)
                {
                    throw new ValidationException("Task title cannot exceed 100 characters");
                }

                if (request.Description?.Length > 500)
                {
                    throw new ValidationException("Task description cannot exceed 500 characters");
                }

                // Check if task exists
                var existingTask = await _taskRepository.GetByIdAsync(id);
                if (existingTask == null)
                {
                    throw new TaskNotFoundException(id);
                }

                existingTask.Title = request.Title;
                existingTask.Description = request.Description;
                existingTask.IsCompleted = request.IsCompleted;
                existingTask.UpdatedAt = DateTime.UtcNow;

                var updatedTask = await _taskRepository.UpdateAsync(existingTask);
                _logger.LogInformation("Task updated with ID {TaskId}", updatedTask.Id);
                
                return MapToResponse(updatedTask);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (TaskNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with ID {TaskId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("Task ID must be a positive integer");
                }

                var deleted = await _taskRepository.DeleteAsync(id);
                if (deleted)
                {
                    _logger.LogInformation("Task deleted with ID {TaskId}", id);
                }
                else
                {
                    _logger.LogWarning("Attempted to delete non-existent task with ID {TaskId}", id);
                }
                
                return deleted;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with ID {TaskId}", id);
                throw;
            }
        }

        public async Task<bool> TaskExistsAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                return await _taskRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if task exists with ID {TaskId}", id);
                throw;
            }
        }

        private static TaskResponse MapToResponse(Models.Task taskEntity)
        {
            return new TaskResponse
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                IsCompleted = taskEntity.IsCompleted,
                CreatedAt = taskEntity.CreatedAt,
                UpdatedAt = taskEntity.UpdatedAt
            };
        }
    }
} 