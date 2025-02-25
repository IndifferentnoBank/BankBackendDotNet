using CoreService.Domain.Entities;

namespace CoreService.Application.Helpers.TransactionExecutor;

public interface ITransactionExecutor
{
    Task ExecuteTransaction(Transaction transaction);
}