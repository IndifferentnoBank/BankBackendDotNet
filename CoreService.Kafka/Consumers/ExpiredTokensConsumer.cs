using System.Text.Json;
using Common.Kafka.Configs;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Domain.Entities;
using CoreService.Contracts.Events;
using CoreService.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Consumers;

public class ExpiredTokensConsumer : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IExpiredTokensRepository _expiredTokensRepository;
    private readonly ILogger<ExpiredTokensConsumer> _logger;

    public ExpiredTokensConsumer(
        IOptions<ExpiredTokensConsumerConfig> configOptions,
        IExpiredTokensRepository repo,
        ILogger<ExpiredTokensConsumer> logger)
    {
        var config = configOptions.Value;
        _expiredTokensRepository = repo;
        _logger = logger;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = config.BootstrapServers,
            GroupId = config.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _consumer.Subscribe(config.Topic);
    }


    public Task ConsumeAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);

                _logger.LogInformation("Consumed message {Message}", result.Message.Value);

                var token = JsonSerializer.Deserialize<ExpiredTokenEvent>(result.Message.Value);

                await _expiredTokensRepository.AddAsync(new ExpiredToken
                {
                    Key = token.Key,
                });
            }
        }, cancellationToken);
    }
}