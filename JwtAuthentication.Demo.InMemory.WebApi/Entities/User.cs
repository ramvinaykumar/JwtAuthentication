namespace JwtAuthentication.Demo.InMemory.WebApi.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }=string.Empty;

        public string PasswordHash { get; set; }= string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
