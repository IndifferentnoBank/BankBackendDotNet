using Microsoft.AspNetCore.SignalR;

namespace CoreService.Infrastructure.SignalR;

public class TransactionHub : Hub
{
    public async Task SendTransactionUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveTransactionUpdate", message);
    }
}