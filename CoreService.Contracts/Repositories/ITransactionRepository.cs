using Common.GenericRepository;
using CoreService.Domain.Enums;
using Transaction = CoreService.Domain.Entities.Transaction;

namespace CoreService.Contracts.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<List<Transaction>> GetAllByAccountIdAsync(Guid accountId);
    Task<Transaction?> GetByIdAsync(Guid transactionId);
    Task<List<Transaction>> GetByStatusAsync(TransactionStatus status);
}