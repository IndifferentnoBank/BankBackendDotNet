using Common.Kafka.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreService.Kafka;

public class KafkaBackgroundService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IKafkaConsumer>();
        consumer.ConsumeAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}