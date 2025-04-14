using System.Text.Json;
using Common.Configurations;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Domain.Entities;
using CoreService.Contracts.Events;
using CoreService.Contracts.Repositories;
using CoreService.Persistence.Repositories.ExpiredTokensRepository;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Consumers;

public class ExpiredTokensConsumer : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IExpiredTokensRepository _expiredTokensRepository;

    public ExpiredTokensConsumer(IOptions<KafkaConfiguration> kafkaConfigOptions, IExpiredTokensRepository expiredTokensRepository)
    {
        _expiredTokensRepository = expiredTokensRepository;
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
                var token = JsonSerializer.Deserialize<ExpiredTokenEvent>(result.Message.Value);

                await _expiredTokensRepository.AddAsync(new ExpiredToken
                {
                    Key = token.Key,
                    ExpirationDate = token.ExpirationDate
                });
            }
        }, cancellationToken);
    }
}