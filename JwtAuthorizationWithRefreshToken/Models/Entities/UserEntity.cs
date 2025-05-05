using System.ComponentModel.DataAnnotations;

namespace JwtAuthorizationWithRefreshToken.Models.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
