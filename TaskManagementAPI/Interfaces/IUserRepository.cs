using TaskManagementAPI.Models;

namespace TaskManagementAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<User> CreateAsync(User user);
        Task<bool> ExistsByUsernameAsync(string username);
    }
} 