using System.Transactions;
using Common.GenericRepository;
using Transaction = CoreService.Domain.Entities.Transaction;

namespace CoreService.Persistence.Repositories.TransactionsRepository;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<List<Transaction>> GetAllByAccountIdAsync(Guid accountId);
    Task<Transaction?> GetByIdAsync(Guid transactionId);
}