using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CoreService.Infrastructure.SignalR;

public class TransactionNotifier : ITransactionNotifier
{
    private readonly IHubContext<TransactionHub> _hubContext;

    private const string AllTransactionsGroup = "AllTransactions";

    public TransactionNotifier(IHubContext<TransactionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendTransactionUpdate(TransactionDto transaction)
    {
        await _hubContext.Clients.Group(AllTransactionsGroup)
            .SendAsync("ReceiveTransactionUpdate", transaction);
    }

    public async Task SendTransactionUpdateToBankAccount(Guid bankAccountId, TransactionDto transaction)
    {
        string groupName = GetGroupNameForBankAccount(bankAccountId);
        await _hubContext.Clients.Group(groupName)
            .SendAsync("ReceiveTransactionUpdateByBankAccountId", transaction);
    }

    private static string GetGroupNameForBankAccount(Guid bankAccountId)
    {
        return $"BankAccount:{bankAccountId}";
    }
}