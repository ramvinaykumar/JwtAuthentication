using JwtAuthorizationDemo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizationDemo.Data
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
        public DbSet<UserEN> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEN>()
                .HasKey(u => u.UserId);  // Explicitly define the primary key
        }

    }
}
