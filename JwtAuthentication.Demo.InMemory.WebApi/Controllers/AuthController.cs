using JwtAuthentication.Demo.InMemory.WebApi.Interfaces;
using JwtAuthentication.Demo.InMemory.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Demo.InMemory.WebApi.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related actions such as user registration and login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Service for handling JWT authentication and user management.
        /// </summary>
        private readonly IJwtAuthServices _jwtAuthServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtAuthServices"></param>
        public AuthController(IJwtAuthServices jwtAuthServices)
        {
            _jwtAuthServices = jwtAuthServices;
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserRegisterDto request)
        {
            try
            {
                var result = await _jwtAuthServices.RegisterUserAsync(request);
                if (result == null)
                {
                    return BadRequest("User already exists");
                }
                return Ok(new
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs in a user with the provided login details and returns a JWT token if successful.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto request)
        {
            try
            {
                var result = await _jwtAuthServices.LoginUserAsync(request);

                if(result?.UserDetail == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid username or password",
                        Data = result
                    });
                }
                return Ok(new
                {
                    Success = true,
                    Message = "Login successful",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the user is authorized.
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-autherization")]
        [Authorize]
        public ActionResult<string> CheckAutherization()
        {
            try
            {
                return Ok("You are authorized");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the user is authorized with a specific role.
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-authorization-with-role")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> CheckAuthorizationWithRole()
        {
            try
            {
                return Ok("You are authorized with Admin role");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Refreshes the JWT token using the provided refresh token.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto requestDto)
        {
            try
            {
                var result = await _jwtAuthServices.RefreshTokenAsync(requestDto);
                return Ok(new
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a user with the specified username exists in the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("is-user-exists-")]
        public async Task<ActionResult<bool>> IsUserExists(string username)
        {
            try
            {
                var result = await _jwtAuthServices.IsUserExistsAsync(username);
                return Ok(new
                {
                    Success = result == true ? true : false,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
