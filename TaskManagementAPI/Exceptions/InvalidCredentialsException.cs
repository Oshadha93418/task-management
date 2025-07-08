namespace TaskManagementAPI.Exceptions
{
    public class InvalidCredentialsException : TaskManagementException
    {
        public InvalidCredentialsException(string message) : base(message)
        {
        }
    }
} 