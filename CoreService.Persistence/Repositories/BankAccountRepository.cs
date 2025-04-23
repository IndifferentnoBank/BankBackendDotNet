using Common.GenericRepository;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreService.Persistence.Repositories;

public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
{
    private readonly CoreServiceDbContext _dbContext;
    private readonly string _masterAccountName;


    public BankAccountRepository(CoreServiceDbContext context, IConfiguration configuration) : base(context)
    {
        _dbContext = context;
        _masterAccountName = configuration["MasterAccount:AccountName"]!;
    }

    public async Task<BankAccount> GetByIdAsync(Guid id)
    {
        return (await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == id))!;
    }

    public async Task<List<BankAccount>> GetAllAccountsByUserIdAsync(Guid userId)
    {
        return await _dbContext.BankAccounts.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<bool> CheckIfBankAccountExistsByAccountNumberAsync(string accountNumber)
    {
        return await _dbContext.BankAccounts.AnyAsync(x => x.AccountNumber == accountNumber);
    }

    public async Task<bool> CheckIfBankAccountExistsByAccountId(Guid id)
    {
        return await _dbContext.BankAccounts.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckIfBankAccountBelongsToUserAsync(Guid bankAccountId, Guid userId)
    {
        return await _dbContext.BankAccounts.AnyAsync(x => x.Id == bankAccountId && x.UserId == userId);
    }

    public Task<IQueryable<BankAccount>> GetAllBankAccountsAsync()
    {
        return Task.FromResult(_dbContext.BankAccounts.AsQueryable());
    }

    public async Task<BankAccount> GetMasterAccountAsync()
    {
        return (await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Name == _masterAccountName))!;
    }

    public async Task<Guid> GetMasterAccountIdAsync()
    {
        var masterAccount = await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Name == _masterAccountName);
        return masterAccount!.Id;
    }
}