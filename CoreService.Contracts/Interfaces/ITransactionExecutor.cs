using CoreService.Domain.Entities;

namespace CoreService.Contracts.Interfaces;

public interface ITransactionExecutor
{
    Task ExecuteTransactionAsync(Transaction transaction);
}