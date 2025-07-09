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

        // Get current user ID from HTTP context
        protected int? GetCurrentUserId()
        {
            if (HttpContext.Items.TryGetValue("CurrentUserId", out var userId))
            {
                return (int)userId;
            }
            return null;
        }

        // Check if user is authenticated
        protected bool IsAuthenticated()
        {
            return GetCurrentUserId().HasValue;
        }
    }
} 