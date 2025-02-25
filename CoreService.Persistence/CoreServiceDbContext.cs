using CoreService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreService.Persistence;

public class CoreServiceDbContext : DbContext
{
    public DbSet<BankAccount> BankAccounts { get; set; }

    public DbSet<Transaction> Transactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasIndex(e => e.AccountNumber)
                .IsUnique();
            
            entity.HasMany(e=> e.Transactions)
                .WithOne(e=>e.BankAccount)
                .HasForeignKey(e=>e.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}