using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<Models.Task?> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<Models.Task> CreateTaskAsync(Models.Task task)
        {
            // Business logic validation
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ValidationException("Task title is required");
            }

            if (task.Title.Length > 100)
            {
                throw new ValidationException("Task title cannot exceed 100 characters");
            }

            if (task.Description?.Length > 500)
            {
                throw new ValidationException("Task description cannot exceed 500 characters");
            }

            return await _taskRepository.CreateAsync(task);
        }

        public async Task<Models.Task> UpdateTaskAsync(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                throw new ValidationException("Task ID mismatch");
            }

            // Business logic validation
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ValidationException("Task title is required");
            }

            if (task.Title.Length > 100)
            {
                throw new ValidationException("Task title cannot exceed 100 characters");
            }

            if (task.Description?.Length > 500)
            {
                throw new ValidationException("Task description cannot exceed 500 characters");
            }

            try
            {
                return await _taskRepository.UpdateAsync(task);
            }
            catch (InvalidOperationException)
            {
                throw new TaskNotFoundException(id);
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _taskRepository.ExistsAsync(id);
        }
    }
} 