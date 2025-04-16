using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserSevice.Persistence
{
    public class UserServiceDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<ExpiredToken> ExpiredTokens { get; set; }

        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
