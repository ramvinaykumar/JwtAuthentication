using JwtAuthorizationWithRefreshToken.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizationWithRefreshToken.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; } = null!;
    }
}
