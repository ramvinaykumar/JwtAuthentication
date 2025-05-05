using JwtAuthentication.Demo.InMemory.WebApi.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Demo.InMemory.WebApi.Helpers
{
    public static class JwtHelpers
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The generated JWT token as a string.</returns>
        public static string GenerateJwtToken(User user, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokens:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["JwtTokens:Issuer"],
                audience: configuration["JwtTokens:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A JWT token as a string.</returns>
        //private async Task<string> GenerateToken(User user)
        //{
        //    var token = string.Empty;
        //    await Task.Run(() =>
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.Username),
        //            new Claim(ClaimTypes.Role, user.Role),
        //                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        //        };
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtTokens:SecretKey")!));
        //        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        //        var tokenDescriptor = new JwtSecurityToken(
        //        issuer: _configuration.GetValue<string>("JwtTokens:Issuer"),
        //        audience: _configuration.GetValue<string>("JwtTokens:Audience"),
        //            claims: claims,
        //            expires: DateTime.Now.AddDays(1),
        //            signingCredentials: cred
        //            );
        //        token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        //    });

        //    return token;
        //}
    }
}
