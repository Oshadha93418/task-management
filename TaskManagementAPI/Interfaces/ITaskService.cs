using TaskManagementAPI.Models;

namespace TaskManagementAPI.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponse>> GetAllTasksAsync();
        Task<TaskResponse?> GetTaskByIdAsync(int id);
        Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
        Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> TaskExistsAsync(int id);
        
        // User-specific methods
        Task<IEnumerable<TaskResponse>> GetTasksByUserIdAsync(int userId);
        Task<TaskResponse?> GetTaskByIdAndUserIdAsync(int id, int userId);
        Task<TaskResponse> CreateTaskForUserAsync(CreateTaskRequest request, int userId);
        Task<TaskResponse> UpdateTaskForUserAsync(int id, UpdateTaskRequest request, int userId);
        Task<bool> DeleteTaskForUserAsync(int id, int userId);
        Task<bool> TaskExistsForUserAsync(int id, int userId);
    }
} 