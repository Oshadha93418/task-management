using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TaskManagementAPI.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Password cannot contain whitespace")]
        public string Password { get; set; } = string.Empty;

        public void SanitizeInput()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                Username = Username.Trim().ToLowerInvariant();
                Username = Regex.Replace(Username, @"\s+", ""); // Remove all whitespace
            }

            if (!string.IsNullOrEmpty(Password))
            {
                Password = Password.Trim();
            }
        }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Password cannot contain whitespace")]
        public string Password { get; set; } = string.Empty;

        public void SanitizeInput()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                Username = Username.Trim().ToLowerInvariant();
                Username = Regex.Replace(Username, @"\s+", ""); // Remove all whitespace
            }

            if (!string.IsNullOrEmpty(Password))
            {
                Password = Password.Trim();
            }
        }
    }

    public class AuthResponse
    {
        public string Message { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
} 