using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;
using Microsoft.Extensions.Logging;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IAuthService authService, ILogger<UsersController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            try
            {
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

                var response = await _authService.LoginAsync(request);
                return Ok(response);
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
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 401,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", request.Username);
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred during login",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            try
            {
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

                var response = await _authService.RegisterAsync(request);
                return CreatedAtAction(nameof(Register), response);
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
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 409,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Username}", request.Username);
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An error occurred during registration",
                    StatusCode = 500,
                    Timestamp = DateTime.UtcNow,
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }
    }


} 