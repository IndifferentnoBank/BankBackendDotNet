using System.Text.Json;
using Common.Configurations;
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

    public ExpiredTokensConsumer(IOptions<KafkaConfiguration> kafkaConfigOptions,
        IExpiredTokensRepository expiredTokensRepository, ILogger<ExpiredTokensConsumer> logger)
    {
        _expiredTokensRepository = expiredTokensRepository;
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaConfigOptions.Value.BootstrapServers,
            GroupId = kafkaConfigOptions.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(kafkaConfigOptions.Value.Topic);
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