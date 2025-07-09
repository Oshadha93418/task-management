using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskManagementAPI.Exceptions;

namespace TaskManagementAPI.Middleware
{
    public class RequestValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestValidationMiddleware> _logger;

        public RequestValidationMiddleware(RequestDelegate next, ILogger<RequestValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Log incoming request
                _logger.LogInformation("Incoming request: {Method} {Path}", 
                    context.Request.Method, context.Request.Path);

                // Validate request size
                if (context.Request.ContentLength > 1024 * 1024) // 1MB limit
                {
                    await WriteErrorResponse(context, 413, "Request too large");
                    return;
                }

                await _next(context);

                // Log response
                _logger.LogInformation("Response: {StatusCode} for {Method} {Path}", 
                    context.Response.StatusCode, context.Request.Method, context.Request.Path);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                await WriteErrorResponse(context, 400, ex.Message);
            }
            catch (TaskNotFoundException ex)
            {
                _logger.LogWarning("Resource not found: {Message}", ex.Message);
                await WriteErrorResponse(context, 404, ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                await WriteErrorResponse(context, 404, ex.Message);
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogWarning("User already exists: {Message}", ex.Message);
                await WriteErrorResponse(context, 409, ex.Message);
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning("Invalid credentials: {Message}", ex.Message);
                await WriteErrorResponse(context, 401, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in middleware");
                await WriteErrorResponse(context, 500, "An unexpected error occurred");
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                Message = message,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow,
                RequestId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
} 