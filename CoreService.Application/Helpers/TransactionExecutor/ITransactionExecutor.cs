using CoreService.Domain.Entities;

namespace CoreService.Application.Helpers.TransactionExecutor;

public interface ITransactionExecutor
{
    Task ExecuteTransactionAsync(Transaction transaction);
}