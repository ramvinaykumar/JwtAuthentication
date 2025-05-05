using JwtAuthorizationDemo.Interfaces;
using JwtAuthorizationDemo.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthorizationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserDataService _userService;

        public ProfileController(IUserDataService userService)
        {
            _userService = userService;
        }

        [HttpGet("my-profile")]
        public async Task<ActionResult<UserProfileResponseDto>> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(); // Should not happen if [Authorize] is correctly configured
            }

            var result = await _userService.GetUserProfileAsync(Guid.Parse(userIdClaim));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return NotFound(result.Error);
            }
        }
    }
}
