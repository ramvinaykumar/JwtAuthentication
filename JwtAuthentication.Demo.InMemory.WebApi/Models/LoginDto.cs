namespace JwtAuthentication.Demo.InMemory.WebApi.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login details.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
