using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreService.Kafka.Consumers.Transactions;

public class TransactionConsumerService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<TransactionConsumerService> _logger;
    private CancellationTokenSource? _cts;
    private Task? _backgroundTask;

    public TransactionConsumerService(ILogger<TransactionConsumerService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TransactionConsumerService is starting.");
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        _backgroundTask =
            Task.Run(
                () => _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TransactionConsumer>()
                    .ConsumeAsync(_cts.Token), _cts.Token);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TransactionConsumerService is stopping.");

        if (_cts != null)
        {
            _cts.Cancel();

            if (_backgroundTask != null)
            {
                try
                {
                    await _backgroundTask;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("TransactionConsumerService task was cancelled.");
                }
            }
        }
    }
}