using Common.GenericRepository;
using CoreService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreService.Persistence.Repositories.BankAccountRepository;

public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
{
    private readonly CoreServiceDbContext _dbContext;
    
    public BankAccountRepository(CoreServiceDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<BankAccount> GetByIdAsync(Guid id)
    {
        return (await _dbContext.BankAccounts.FirstOrDefaultAsync(x=> x.Id == id))!;
    }

    public async Task<List<BankAccount>> GetAllAccountsByUserIdAsync(Guid userId)
    {
       return await _dbContext.BankAccounts.Where(x=>x.UserId == userId).ToListAsync();
    }

    public async Task<bool> CheckIfBankAccountExistsByAccountNumberAsync(string accountNumber)
    {
        return await _dbContext.BankAccounts.AnyAsync(x=>x.AccountNumber == accountNumber);
    }

    public async Task<bool> CheckIfBankAccountExistsByAccountId(Guid id)
    {
        return await _dbContext.BankAccounts.AnyAsync(x=>x.Id == id);
    }

    public async Task<bool> CheckIfBankAccountBelongsToUserAsync(Guid bankAccountId, Guid userId)
    {
        return await _dbContext.BankAccounts.AnyAsync(x=>x.Id == bankAccountId && x.UserId == userId);
    }

    public Task<IQueryable<BankAccount>> GetAllBankAccountsAsync()
    {
        return Task.FromResult(_dbContext.BankAccounts.AsQueryable());
    }
}