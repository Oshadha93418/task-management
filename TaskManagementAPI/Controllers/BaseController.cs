using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IAuthService _authService;

        protected BaseController(IAuthService authService)
        {
            _authService = authService;
        }

        // Authentication methods are no longer needed without JWT
        // Controllers will handle authentication differently if needed
    }
} 