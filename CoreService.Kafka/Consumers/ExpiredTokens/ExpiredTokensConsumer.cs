using System.Text.Json;
using Common.Kafka.Configs;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Consumers.ExpiredTokens;

public class ExpiredTokensConsumer : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExpiredTokensConsumer> _logger;

    public ExpiredTokensConsumer(
        IOptions<ExpiredTokensConsumerConfig> configOptions,
        ILogger<ExpiredTokensConsumer> logger, IServiceScopeFactory scopeFactory)
    {
        var config = configOptions.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = config.BootstrapServers,
            GroupId = config.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = config.EnableAutoCommit,
            EnableAutoOffsetStore = config.EnableAutoOffsetStore
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _consumer.Subscribe(config.Topic);

        _logger.LogInformation("ExpiredTokensConsumer initialized with topic: {Topic}", config.Topic);
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(cancellationToken);

                    _logger.LogInformation("Consumed message: {Message}", result.Message.Value);

                    var tokenEvent = JsonSerializer.Deserialize<ExpiredTokenEvent>(result.Message.Value);

                    var expiredToken = new ExpiredToken
                    {
                        Key = tokenEvent.DeletedToken
                    };

                    var tokensRepository = _scopeFactory.CreateScope().ServiceProvider
                        .GetRequiredService<IExpiredTokensRepository>();

                    if (!await tokensRepository.CheckIfTokenAlreadyExists(tokenEvent.DeletedToken))
                    {
                        await tokensRepository.AddAsync(expiredToken);
                        _logger.LogInformation("Added expired token with key: {Key}", tokenEvent.DeletedToken);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer canceled.");
        }
        finally
        {
            _consumer.Close();
            _logger.LogInformation("ExpiredTokensConsumer closed.");
        }
    }
}