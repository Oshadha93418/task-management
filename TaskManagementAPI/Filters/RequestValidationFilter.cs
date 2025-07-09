using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagementAPI.Models;
using System.Text.RegularExpressions;

namespace TaskManagementAPI.Filters
{
    public class RequestValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Validate request size
            if (context.HttpContext.Request.ContentLength > 1024 * 1024) // 1MB limit
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    Message = "Request too large",
                    StatusCode = 413,
                    Timestamp = DateTime.UtcNow,
                    RequestId = context.HttpContext.TraceIdentifier
                });
                return;
            }

            // Sanitize string inputs
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is CreateTaskRequest createRequest)
                {
                    createRequest.SanitizeInput();
                }
                else if (argument is UpdateTaskRequest updateRequest)
                {
                    updateRequest.SanitizeInput();
                }
                else if (argument is LoginRequest loginRequest)
                {
                    loginRequest.SanitizeInput();
                }
                else if (argument is RegisterRequest registerRequest)
                {
                    registerRequest.SanitizeInput();
                }
            }

            // Validate content type for POST/PUT requests
            if (context.HttpContext.Request.Method == "POST" || context.HttpContext.Request.Method == "PUT")
            {
                var contentType = context.HttpContext.Request.ContentType;
                if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith("application/json"))
                {
                    context.Result = new BadRequestObjectResult(new ErrorResponse
                    {
                        Message = "Content-Type must be application/json",
                        StatusCode = 400,
                        Timestamp = DateTime.UtcNow,
                        RequestId = context.HttpContext.TraceIdentifier
                    });
                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Log successful requests
            if (context.Exception == null)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<RequestValidationFilter>>();
                logger?.LogInformation("Request completed successfully: {Method} {Path}", 
                    context.HttpContext.Request.Method, context.HttpContext.Request.Path);
            }
        }
    }
} 