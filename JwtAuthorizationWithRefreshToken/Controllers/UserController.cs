using JwtAuthorizationWithRefreshToken.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JwtAuthorizationWithRefreshToken.Controllers
{
    /// <summary>
    /// Controller for handling user-related operations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Service for handling authentication and user management.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="authService"></param>
        public UserController(IAuthService authService) { _authService = authService; }

        /// <summary>
        /// Fetches the profile of the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserID"]);
            var user = await _authService.GetUserByIdAsync(userId);
            return Ok(new { Status = HttpStatusCode.OK, Data = user, Message = "User Profile Fetched Successfully!" });
        }
    }
}
