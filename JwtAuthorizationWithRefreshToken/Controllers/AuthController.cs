using JwtAuthorizationWithRefreshToken.Interfaces;
using JwtAuthorizationWithRefreshToken.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JwtAuthorizationWithRefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) { _authService = authService; }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            return response != null ? Ok(new { Status = HttpStatusCode.OK, Data = response, Message = "User Registered Successfully!"}) : BadRequest("Registration failed!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return response != null ? Ok(new { Status = HttpStatusCode.OK, Data = response, Message = "Login Successfully!" }) : Unauthorized();
        }

        [HttpPost("refresh/{refreshToken}")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var response = await _authService.RefreshTokenAsync(refreshToken);
            return response != null ? Ok(new { Status = HttpStatusCode.OK, Data = response, Message = "RefreshToken Done Successfully!" }) : Unauthorized();
        }
    }
}
