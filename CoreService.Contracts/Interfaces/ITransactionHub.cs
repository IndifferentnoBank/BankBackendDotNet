using CoreService.Contracts.ExternalDtos;

namespace CoreService.Contracts.Interfaces;

public interface ITransactionHub
{
    public Task JoinAllTransactionsGroup();
    public Task LeaveAllTransactionsGroup();
    public Task JoinBankAccountGroup(Guid bankAccountId);
    public Task LeaveBankAccountGroup(Guid bankAccountId);

}