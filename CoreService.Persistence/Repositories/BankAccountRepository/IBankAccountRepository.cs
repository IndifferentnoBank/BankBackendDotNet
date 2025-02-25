using Common.GenericRepository;
using CoreService.Domain.Entities;

namespace CoreService.Persistence.Repositories.BankAccountRepository;

public interface IBankAccountRepository : IGenericRepository<BankAccount>
{
    Task<BankAccount> GetByIdAsync(Guid id);
    Task<List<BankAccount>> GetAllAccountsByUserIdAsync(Guid userId);
    Task<bool> CheckIfBankAccountExistsByAccountNumberAsync(string accountNumber);
    Task<bool> CheckIfBankAccountExistsByAccountId(Guid id);
}