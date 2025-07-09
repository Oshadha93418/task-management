namespace TaskManagementAPI.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string RequestId { get; set; } = string.Empty;
        public List<string>? Details { get; set; }
    }
} 