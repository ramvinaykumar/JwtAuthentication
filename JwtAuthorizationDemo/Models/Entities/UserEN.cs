namespace JwtAuthorizationDemo.Models.Entities
{
    public class UserEN
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty; // User, Admin, etc.
    }
}
