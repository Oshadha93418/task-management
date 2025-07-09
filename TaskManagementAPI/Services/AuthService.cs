using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace TaskManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return false;
                }

                var user = await _userRepository.GetByUsernameAndPasswordAsync(username, password);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating credentials for user {Username}", username);
                return false;
            }
        }

        public async Task<User?> GetUserAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return null;
                }

                return await _userRepository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {Username}", username);
                throw;
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Sanitize input
                request.SanitizeInput();

                // Additional validation
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    throw new ValidationException("Username is required");
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new ValidationException("Password is required");
                }

                if (request.Username.Length < 3 || request.Username.Length > 50)
                {
                    throw new ValidationException("Username must be between 3 and 50 characters");
                }

                if (request.Password.Length < 6 || request.Password.Length > 100)
                {
                    throw new ValidationException("Password must be between 6 and 100 characters");
                }

                // Validate username format
                if (!Regex.IsMatch(request.Username, @"^[a-zA-Z0-9_-]+$"))
                {
                    throw new ValidationException("Username can only contain letters, numbers, underscores, and hyphens");
                }

                var user = await _userRepository.GetByUsernameAndPasswordAsync(request.Username, request.Password);
                
                if (user == null)
                {
                    _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
                    throw new InvalidCredentialsException("Invalid username or password");
                }

                _logger.LogInformation("Successful login for user {Username}", request.Username);

                return new AuthResponse
                {
                    Message = "Login successful",
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        CreatedAt = user.CreatedAt
                    }
                };
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (InvalidCredentialsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", request.Username);
                throw;
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Sanitize input
                request.SanitizeInput();

                // Additional validation
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    throw new ValidationException("Username is required");
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new ValidationException("Password is required");
                }

                if (request.Username.Length < 3 || request.Username.Length > 50)
                {
                    throw new ValidationException("Username must be between 3 and 50 characters");
                }

                if (request.Password.Length < 6 || request.Password.Length > 100)
                {
                    throw new ValidationException("Password must be between 6 and 100 characters");
                }

                // Validate username format
                if (!Regex.IsMatch(request.Username, @"^[a-zA-Z0-9_-]+$"))
                {
                    throw new ValidationException("Username can only contain letters, numbers, underscores, and hyphens");
                }

                // Validate password strength (basic)
                if (!Regex.IsMatch(request.Password, @"^[^\s]+$"))
                {
                    throw new ValidationException("Password cannot contain whitespace");
                }

                // Check if user already exists
                var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration attempt with existing username {Username}", request.Username);
                    throw new UserAlreadyExistsException(request.Username);
                }

                var user = new User
                {
                    Username = request.Username,
                    Password = request.Password,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.CreateAsync(user);

                _logger.LogInformation("New user registered: {Username}", request.Username);

                return new AuthResponse
                {
                    Message = "Registration successful",
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        CreatedAt = user.CreatedAt
                    }
                };
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (UserAlreadyExistsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Username}", request.Username);
                throw;
            }
        }
    }
} 