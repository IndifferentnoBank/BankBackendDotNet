using Microsoft.AspNetCore.SignalR;

namespace CoreService.Infrastructure.SignalR;

public class TransactionHub : Hub
{
    private const string AllTransactionsGroup = "AllTransactions";

    public async Task SendTransactionUpdate(TransactionDto transaction)
    {
        await Clients.Group(AllTransactionsGroup).SendAsync("ReceiveTransactionUpdate", transaction);
    }

    public async Task SendTransactionUpdateToBankAccount(Guid bankAccountId, TransactionDto transaction)
    {
        string groupName = GetGroupNameForBankAccount(bankAccountId);
        await Clients.Group(groupName).SendAsync("ReceiveTransactionUpdateByBankAccountId", transaction);
    }

    public async Task JoinAllTransactionsGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, AllTransactionsGroup);
        await Clients.Caller.SendAsync("JoinedGroup", $"You have joined the group for All Transactions.");
    }

    public async Task LeaveAllTransactionsGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, AllTransactionsGroup);
        await Clients.Caller.SendAsync("LeftGroup", $"You have left the group for All Transactions.");
    }

    public async Task JoinBankAccountGroup(Guid bankAccountId)
    {
        string groupName = GetGroupNameForBankAccount(bankAccountId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("JoinedGroup", $"You have joined the group for Bank Account ID: {bankAccountId}");
    }

    public async Task LeaveBankAccountGroup(Guid bankAccountId)
    {
        string groupName = GetGroupNameForBankAccount(bankAccountId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("LeftGroup", $"You have left the group for Bank Account ID: {bankAccountId}");
    }

    private static string GetGroupNameForBankAccount(Guid bankAccountId)
    {
        return $"BankAccount:{bankAccountId}";
    }
}