using TaskManagementAPI.Models;

namespace TaskManagementAPI.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> GetAllAsync();
        Task<Models.Task?> GetByIdAsync(int id);
        Task<Models.Task> CreateAsync(Models.Task task);
        Task<Models.Task> UpdateAsync(Models.Task task);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // User-specific methods
        Task<IEnumerable<Models.Task>> GetByUserIdAsync(int userId);
        Task<Models.Task?> GetByIdAndUserIdAsync(int id, int userId);
        Task<bool> ExistsForUserAsync(int id, int userId);
    }
} 