using JwtAuthorizationDemo.Models.DTOs;

namespace JwtAuthorizationDemo.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(UserProfileResponseDto user);
    }
}
