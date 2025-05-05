namespace JwtAuthorizationDemo.Models.DTOs
{
    public class AuthenticationResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
    }
}
