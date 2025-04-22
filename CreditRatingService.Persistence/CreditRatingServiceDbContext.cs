using Microsoft.EntityFrameworkCore;
using CreditRatingService.Domain.Entities;

namespace CreditRatingService.Persistence
{
    public class CreditRatingServiceDbContext : DbContext
    {
        public DbSet<ExpiredToken> ExpiredTokens { get; set; }

        public CreditRatingServiceDbContext(DbContextOptions<CreditRatingServiceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
