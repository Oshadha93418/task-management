namespace TaskManagementAPI.Exceptions
{
    public class TaskManagementException : Exception
    {
        public TaskManagementException(string message) : base(message)
        {
        }

        public TaskManagementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class TaskNotFoundException : TaskManagementException
    {
        public TaskNotFoundException(int taskId) : base($"Task with ID {taskId} not found")
        {
        }
    }

    public class UserNotFoundException : TaskManagementException
    {
        public UserNotFoundException(string username) : base($"User '{username}' not found")
        {
        }
    }

    public class UserAlreadyExistsException : TaskManagementException
    {
        public UserAlreadyExistsException(string username) : base($"User '{username}' already exists")
        {
        }
    }

    public class ValidationException : TaskManagementException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
} 