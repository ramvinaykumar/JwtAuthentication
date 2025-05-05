using JwtAuthorizationDemo.Models.DTOs;
using JwtAuthorizationDemo.Models.Entities;

namespace JwtAuthorizationDemo.Interfaces
{
    public interface IUserDataService
    {
        Task<ResultDto<UserEN>> RegisterUserAsync(UserRegisterDto requestDto);

        Task<ResultDto<string>> LoginAsync(string username, string password);

        Task<ResultDto<UserProfileResponseDto>> GetUserProfileAsync(Guid userId);

        Task<ResultDto<UserProfileResponseDto>> GetUserByIdAsync(Guid userId);

        Task<ResultDto<UserProfileResponseDto>> GetUserByUsernameAsync(string username);
    }
}
