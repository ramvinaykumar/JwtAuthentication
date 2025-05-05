using JwtAuthorizationDemo.Data;
using JwtAuthorizationDemo.Interfaces;
using JwtAuthorizationDemo.Models.DTOs;
using JwtAuthorizationDemo.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizationDemo.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly JwtDbContext _dbContext;
        private readonly IPasswordHasher<UserEN> _passwordHasher;

        public UserDataService(JwtDbContext dbContext, IPasswordHasher<UserEN> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultDto<UserProfileResponseDto>> GetUserByIdAsync(Guid userId)
        {
            return await GetUserDataByIdAsync(userId);
        }

        public async Task<ResultDto<UserProfileResponseDto>> GetUserByUsernameAsync(string username)
        {
            var user = await _dbContext.Users.FindAsync(username);
            if (user == null)
            {
                return ResultDto<UserProfileResponseDto>.Failure("User not found.");
            }

            return ResultDto<UserProfileResponseDto>.Success(new UserProfileResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            });
        }

        public async Task<ResultDto<UserProfileResponseDto>> GetUserProfileAsync(Guid userId)
        {
            //var user = await _dbContext.Users.FindAsync(userId);
            //if (user == null)
            //{
            //    return ResultDto<UserProfileResponseDto>.Failure("User not found.");
            //}

            //return ResultDto<UserProfileResponseDto>.Success(new UserProfileResponseDto 
            //{ 
            //    UserId = userId,
            //    UserName = user.UserName,
            //    Email = user.Email,
            //    Role = user.Role
            //});

            return await GetUserDataByIdAsync(userId);
        }

        public async Task<ResultDto<string>> LoginAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null || !_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password).Equals(PasswordVerificationResult.Success))
            {
                return ResultDto<string>.Failure("Invalid username or password.");
            }

            return ResultDto<string>.Success(user.UserId.ToString()); // Return user ID for token generation
        }

        public async Task<ResultDto<UserEN>> RegisterUserAsync(UserRegisterDto requestDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == requestDto.UserName))
            {
                return ResultDto<UserEN>.Failure("Username already exists.");
            }

            var newUser = new UserEN { UserName = requestDto.UserName, Email = requestDto.Email, Role = requestDto.Role };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, requestDto.Password);

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return ResultDto<UserEN>.Success(newUser);
        }

        private async Task<ResultDto<UserProfileResponseDto>> GetUserDataByIdAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return ResultDto<UserProfileResponseDto>.Failure("User not found.");
            }

            return ResultDto<UserProfileResponseDto>.Success(new UserProfileResponseDto
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            });
        }
    }
}
