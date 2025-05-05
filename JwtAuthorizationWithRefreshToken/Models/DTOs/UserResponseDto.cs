namespace JwtAuthorizationWithRefreshToken.Models.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;
    }
}
