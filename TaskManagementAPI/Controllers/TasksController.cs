using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;
using Microsoft.Extensions.Logging;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IAuthService authService, ITaskService taskService, ILogger<TasksController> logger) 
            : base(authService)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasks()
        {
            try
            {
                // Check if user is authenticated
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Authentication required. Please provide X-User-Id header",
                        StatusCode = 401,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                var tasks = await _taskService.GetTasksByUserIdAsync(userId.Value);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred while retrieving tasks",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse>> GetTask(int id)
        {
            try
            {
                // Check if user is authenticated
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Authentication required. Please provide X-User-Id header",
                        StatusCode = 401,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Message = "Task ID must be a positive integer",
                        StatusCode = 400,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                var task = await _taskService.GetTaskByIdAndUserIdAsync(id, userId.Value);
                if (task == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Task not found",
                        StatusCode = 404,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                return Ok(task);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 400,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task with ID {TaskId} for user {UserId}", id, GetCurrentUserId());
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred while retrieving the task",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskResponse>> CreateTask(CreateTaskRequest request)
        {
            try
            {
                // Check if user is authenticated
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Authentication required. Please provide X-User-Id header",
                        StatusCode = 401,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ErrorResponse
                    {
                        Message = "Validation failed",
                        StatusCode = 400,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier,
                        Details = errors
                    });
                }

                var createdTask = await _taskService.CreateTaskForUserAsync(request, userId.Value);
                return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 400,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred while creating the task",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponse>> UpdateTask(int id, UpdateTaskRequest request)
        {
            try
            {
                // Check if user is authenticated
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Authentication required. Please provide X-User-Id header",
                        StatusCode = 401,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Message = "Task ID must be a positive integer",
                        StatusCode = 400,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ErrorResponse
                    {
                        Message = "Validation failed",
                        StatusCode = 400,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier,
                        Details = errors
                    });
                }

                var updatedTask = await _taskService.UpdateTaskForUserAsync(id, request, userId.Value);
                return Ok(updatedTask);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 400,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (TaskNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Task not found",
                    StatusCode = 404,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with ID {TaskId} for user {UserId}", id, GetCurrentUserId());
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred while updating the task",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                // Check if user is authenticated
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Authentication required. Please provide X-User-Id header",
                        StatusCode = 401,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Message = "Task ID must be a positive integer",
                        StatusCode = 400,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                var deleted = await _taskService.DeleteTaskForUserAsync(id, userId.Value);
                if (!deleted)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Task not found",
                        StatusCode = 404,
                        Timestamp = DateTime.UtcNow,
                        RequestId = HttpContext.TraceIdentifier
                    });
                }

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 400,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with ID {TaskId} for user {UserId}", id, GetCurrentUserId());
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred while deleting the task",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }
    }
} 