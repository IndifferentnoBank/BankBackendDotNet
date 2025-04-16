using CoreService.Contracts.Interfaces;
using CoreService.Domain.Entities;
using CoreService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreService.Persistence.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly CoreServiceDbContext _dbContext;
    private readonly IBankAccountNumberGenerator _accountNumberGenerator;
    private readonly IConfiguration _configuration;

    public DbInitializer(CoreServiceDbContext dbContext, IConfiguration configuration,
        IBankAccountNumberGenerator accountNumberGenerator)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _accountNumberGenerator = accountNumberGenerator;
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();

        var masterAccountName = _configuration["MasterAccount:AccountName"] ?? "MasterAccount";

        var existing = await _dbContext.BankAccounts
            .FirstOrDefaultAsync(x => x.Name == masterAccountName);

        if (existing == null)
        {
            var initMoney = decimal.Parse(_configuration["MasterAccount:InitMoney"] ?? "0");

            var account = new BankAccount(
                userId: Guid.Empty,
                name: masterAccountName,
                accountNumber: _accountNumberGenerator.GenerateBankAccountNumber(),
                currency: Currency.RUB
            )
            {
                Balance = initMoney
            };

            _dbContext.BankAccounts.Add(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}