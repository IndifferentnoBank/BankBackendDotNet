using CoreService.Domain.Entities;

namespace CoreService.Contracts.Interfaces;

public interface IFireBaseSender
{
    Task SendMessageToClientAsync(Guid userId, Transaction transaction);
    Task SendMessageToStaffAsync(Transaction transaction);
}