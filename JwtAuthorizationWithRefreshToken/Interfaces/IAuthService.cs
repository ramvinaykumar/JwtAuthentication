using JwtAuthorizationWithRefreshToken.Models.DTOs;

namespace JwtAuthorizationWithRefreshToken.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(UserRegisterDto registerDto);

        Task<AuthResponse?> LoginAsync(LoginDto loginDto);

        Task<AuthResponse?> RefreshTokenAsync(string refreshToken);

        Task<UserResponseDto> GetUserByIdAsync(int userId);
    }
}
