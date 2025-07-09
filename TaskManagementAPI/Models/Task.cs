using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TaskManagementAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Task title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Task title must be between 1 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,!?()]+$", ErrorMessage = "Task title contains invalid characters")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Task description cannot exceed 500 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,!?()\n\r]+$", ErrorMessage = "Task description contains invalid characters")]
        public string? Description { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void SanitizeInput()
        {
            if (!string.IsNullOrEmpty(Title))
            {
                Title = Title.Trim();
                Title = Regex.Replace(Title, @"\s+", " "); // Replace multiple spaces with single space
            }

            if (!string.IsNullOrEmpty(Description))
            {
                Description = Description.Trim();
                Description = Regex.Replace(Description, @"\s+", " "); // Replace multiple spaces with single space
            }
        }
    }
} 