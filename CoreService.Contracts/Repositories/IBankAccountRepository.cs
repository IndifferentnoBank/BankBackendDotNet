using Common.GenericRepository;
using CoreService.Domain.Entities;

namespace CoreService.Contracts.Repositories;

public interface IBankAccountRepository : IGenericRepository<BankAccount>
{
    Task<BankAccount> GetByIdAsync(Guid id);
    Task<List<BankAccount>> GetAllAccountsByUserIdAsync(Guid userId);
    Task<bool> CheckIfBankAccountExistsByAccountNumberAsync(string accountNumber);
    Task<bool> CheckIfBankAccountExistsByAccountId(Guid id);
    Task<bool> CheckIfBankAccountBelongsToUserAsync(Guid bankAccountId, Guid userId);
    Task<IQueryable<BankAccount>> GetAllBankAccountsAsync();
    Task<BankAccount> GetMasterAccountAsync();
    Task<Guid> GetMasterAccountIdAsync();
}