using CoreService.Application.Helpers.TransactionExecutor;
using CoreService.Domain.Enums;
using CoreService.Persistence.Repositories.TransactionsRepository;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CoreService.Application.BackgroundService;

public class TransactionRetryJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TransactionRetryJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var transactionExecutor = scope.ServiceProvider.GetRequiredService<ITransactionExecutor>();
        var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
        
        var pendingTransactions = await transactionRepository.GetByStatusAsync(TransactionStatus.Rejected);
        foreach (var transaction in pendingTransactions)
        {
            await transactionExecutor.ExecuteTransactionAsync(transaction);
        }
    }
}

