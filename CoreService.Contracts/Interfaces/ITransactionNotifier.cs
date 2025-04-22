using CoreService.Contracts.ExternalDtos;

namespace CoreService.Contracts.Interfaces;

public interface ITransactionNotifier
{
    Task SendTransactionUpdate(TransactionDto transaction);
    Task SendTransactionUpdateToBankAccount(Guid bankAccountId, TransactionDto transaction);
}