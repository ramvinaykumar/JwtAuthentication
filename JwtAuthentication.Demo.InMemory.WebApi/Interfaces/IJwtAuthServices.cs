using JwtAuthentication.Demo.InMemory.WebApi.Models;

namespace JwtAuthentication.Demo.InMemory.WebApi.Interfaces
{
    /// <summary>
    /// Interface for JWT authentication services.
    /// </summary>
    public interface IJwtAuthServices
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="userRequestDto"></param>
        /// <returns></returns>
        Task<UserResponseDto> RegisterUserAsync(UserRegisterDto userRequestDto);

        /// <summary>
        /// Logs in a user asynchronously.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        Task<LoginResponseDto?> LoginUserAsync(LoginDto loginDto);

        /// <summary>
        /// Checks if a user with the specified username exists in the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> IsUserExistsAsync(string username);

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto requestDto);
    }
}
