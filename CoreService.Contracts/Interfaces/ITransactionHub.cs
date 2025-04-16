using CoreService.Contracts.ExternalDtos;

namespace CoreService.Contracts.Interfaces;

public interface ITransactionHub
{
    public Task SendTransactionUpdate(TransactionDto transaction);
    public Task SendTransactionUpdateToBankAccount(Guid bankAccountId, TransactionDto transaction);
    public Task JoinAllTransactionsGroup();
    public Task LeaveAllTransactionsGroup();
    public Task JoinBankAccountGroup(Guid bankAccountId);
    public Task LeaveBankAccountGroup(Guid bankAccountId);

}