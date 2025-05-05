namespace JwtAuthentication.Demo.InMemory.WebApi.Models
{
    /// <summary>
    /// Represents the data transfer object for refreshing a token.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public Guid Userid { get; set; }

        /// <summary>
        /// The refresh token used to obtain a new access token.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
