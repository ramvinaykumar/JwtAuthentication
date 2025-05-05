using JwtAuthentication.Demo.InMemory.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Demo.InMemory.WebApi.Data
{
    /// <summary>
    /// Database context for JWT authentication.
    /// </summary>
    public class JwtDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtDbContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        public JwtDbContext(DbContextOptions<JwtDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// DbSet for storing user entities.
        /// </summary>
        public DbSet<User> Users { get; set; }
        
    }
}
