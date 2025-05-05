using JwtAuthorizationDemo.Interfaces;
using JwtAuthorizationDemo.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthorizationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtAuthController : ControllerBase
    {
        private readonly IUserDataService _userService;
        private readonly ITokenService _tokenService;

        public JwtAuthController(IUserDataService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponseDto>> Register(UserRegisterDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResponseDto { IsAuthenticated = false, Message = "Invalid registration data." });
            }

            var result = await _userService.RegisterUserAsync(requestDto);

            if (result.IsSuccess)
            {
                return Ok(new AuthenticationResponseDto { IsAuthenticated = true, Message = "Registration successful." });
            }
            else
            {
                return BadRequest(new AuthenticationResponseDto { IsAuthenticated = false, Message = result.Error });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponseDto>> Login(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResponseDto { IsAuthenticated = false, Message = "Invalid login data." });
            }

            var result = await _userService.LoginAsync(request.Username, request.Password);

            if (result.IsSuccess)
            {
                var user = await _userService.GetUserByIdAsync(Guid.Parse(result.Value));
                if (user == null)
                {
                    return Unauthorized(new AuthenticationResponseDto { IsAuthenticated = false, Message = "Invalid credentials." });
                }
                var token = await _tokenService.GenerateJwtToken(user.Value);

                return Ok(new AuthenticationResponseDto { IsAuthenticated = true, Token = token, Message = "Login successful." });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDto { IsAuthenticated = false, Message = result.Error });
            }
        }
    }
}
