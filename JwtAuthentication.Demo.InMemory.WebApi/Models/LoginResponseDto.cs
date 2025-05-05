namespace JwtAuthentication.Demo.InMemory.WebApi.Models
{
    /// <summary>
    /// Represents the response data transfer object for login operations.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Gets or sets the status of the login operation.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the refresh token for the user.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserResponseDto? UserDetail { get; set; } = new UserResponseDto();
    }
}
