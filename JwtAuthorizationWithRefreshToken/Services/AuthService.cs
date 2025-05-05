using JwtAuthorizationWithRefreshToken.AppDbContext;
using JwtAuthorizationWithRefreshToken.Interfaces;
using JwtAuthorizationWithRefreshToken.Models.DTOs;
using JwtAuthorizationWithRefreshToken.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthorizationWithRefreshToken.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        public async Task<UserResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {
            var user = new UserEntity();
            var hashedPassword = new PasswordHasher<UserEntity>().HashPassword(user, registerDto.Password);
            user.PasswordHash = hashedPassword;
            user.MobileNumber = registerDto.MobileNumber;
            user.EmailAddress = registerDto.EmailAddress;
            user.Username = registerDto.Username;
            user.Role = registerDto.Role;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return await MapEntityToDto(user);
        }

        public async Task<AuthResponse?> LoginAsync(LoginDto loginDto) // Add nullable return type
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == loginDto.Username);
            if (user == null || new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
                return null;

            return await GenerateJwtToken(user);
        }

        public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken) // Add nullable return type
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);
            if (user == null)
            {
                return null;
            }

            return await GenerateJwtToken(user);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            var userData = await _dbContext.Users.FindAsync(userId);
            if (userData == null)
            {
                throw new ArgumentNullException(nameof(userData), "User not found.");
            }
            return await MapEntityToDto(userData);
        }

        private async Task<AuthResponse> GenerateJwtToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpiration"]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var tokenExpiryDays = Convert.ToInt32(_configuration["JwtSettings:RefreshTokenExpiration"]!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(tokenExpiryDays);
            await _dbContext.SaveChangesAsync();

            return new AuthResponse { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
        }

        private async Task<UserResponseDto> MapEntityToDto(UserEntity user)
        {
            var userResponse = new UserResponseDto();

            await Task.Run(() =>
            {
                userResponse.Id = user.Id;
                userResponse.Username = user.Username;
                userResponse.Role = user.Role;
                userResponse.EmailAddress = user.EmailAddress;
                userResponse.MobileNumber = user.MobileNumber;
            });

            return userResponse;
        }
    }
}
