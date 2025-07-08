using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAndPasswordAsync(username, password);
            return user != null;
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAndPasswordAsync(request.Username, request.Password);
            
            if (user == null)
            {
                throw new InvalidCredentialsException("Invalid username or password");
            }

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

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(request.Username);
            }

            var user = new User
            {
                Username = request.Username,
                Password = request.Password
            };

            await _userRepository.CreateAsync(user);

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
    }
} 