using JwtAuthentication.Demo.InMemory.WebApi.Data;
using JwtAuthentication.Demo.InMemory.WebApi.Entities;
using JwtAuthentication.Demo.InMemory.WebApi.Helpers;
using JwtAuthentication.Demo.InMemory.WebApi.Interfaces;
using JwtAuthentication.Demo.InMemory.WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JwtAuthentication.Demo.InMemory.WebApi.Services
{
    /// <summary>
    /// Service for handling JWT authentication and user management.
    /// </summary>
    public class JwtAuthServices : IJwtAuthServices
    {
        private readonly JwtDbContext _dbContext;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtAuthServices"/> class.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        public JwtAuthServices(JwtDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Checks if a user with the specified username exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>A boolean indicating whether the user exists.</returns>
        public async Task<bool> IsUserExistsAsync(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        /// <summary>
        /// Authenticates a user based on the provided login details and generates a JWT token if successful.
        /// </summary>
        /// <param name="loginDto">The login details of the user.</param>
        /// <returns>A tuple containing the user response DTO and a message (JWT token or error message).</returns>
        public async Task<LoginResponseDto?> LoginUserAsync(LoginDto loginDto)
        {
            var response = new LoginResponseDto();
            var message = string.Empty;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(loginDto.Username));
            if (user == null || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
            {
                response.UserDetail = null;
                return response;
            }
            
            return new LoginResponseDto
            {
                UserDetail = await MapEntityToDto(user),
                AccessToken = JwtHelpers.GenerateJwtToken(user, _configuration),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        /// <summary>
        /// Refreshes the JWT token for the user using the provided refresh token.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto requestDto)
        {
            var user = await _dbContext.Users.FindAsync(requestDto.Userid);
            
            if (user is null || user.RefreshToken != requestDto.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return null!;
            }
            var token = JwtHelpers.GenerateJwtToken(user, _configuration);
            var refreshToken = await GenerateAndSaveRefreshToken(user);
            var userData = await MapEntityToDto(user);
            return new LoginResponseDto
            {
                UserDetail = userData,
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// Generates a new refresh token and saves it to the user entity.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var randumNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randumNumber);
            }
            var refreshToken = Convert.ToBase64String(randumNumber);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userRequestDto">The details of the user to register.</param>
        /// <returns>A task representing the asynchronous operation, containing the user response DTO.</returns>
        public async Task<UserResponseDto> RegisterUserAsync(UserRegisterDto userRequestDto)
        {
            if (await IsUserExistsAsync(userRequestDto.Username))
                return null!;

            var user = new User();
            user.Username = userRequestDto.Username;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, userRequestDto.Password);
            user.Role = userRequestDto.Role;
            user.Email = userRequestDto.Email;
            user.FirstName = userRequestDto.FirstName;
            user.LastName = userRequestDto.LastName;
            user.PhoneNumber = userRequestDto.PhoneNumber;
            user.CreatedAt = DateTime.UtcNow;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return await MapEntityToDto(user);
        }

        /// <summary>
        /// Maps a User entity to a UserResponseDto.
        /// </summary>
        /// <param name="user">The user entity to map.</param>
        /// <returns>A UserResponseDto containing the mapped data.</returns>
        private async Task<UserResponseDto?> MapEntityToDto(User user)
        {
            if (user == null)
                return null;

            return await Task.Run(() =>
            {
                return new UserResponseDto
                {
                    UserId = user.UserId.ToString(),
                    Username = user.Username,
                    Role = user.Role,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };
            });
        }
    }
}
