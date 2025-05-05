namespace JwtAuthentication.Demo.InMemory.WebApi.Models
{
    /// <summary>
    /// Represents the data transfer object for user registration.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The role of the user (e.g., Admin, User).
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
