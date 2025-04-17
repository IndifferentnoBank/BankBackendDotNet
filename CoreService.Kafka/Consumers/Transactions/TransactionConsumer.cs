using System.Text.Json;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using CoreService.Kafka.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Consumers.Transactions;

public class TransactionConsumer : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TransactionConsumer> _logger;
    private readonly ITransactionHub _transactionHub;

    public TransactionConsumer(
        IOptions<BankTransactionsConsumerConfig> configOptions,
        ILogger<TransactionConsumer> logger,
        ITransactionHub hub, IServiceScopeFactory scopeFactory)
    {
        var config = configOptions.Value;

        _logger = logger;
        _transactionHub = hub;
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

        _logger.LogInformation("TransactionConsumer initialized with topic: {Topic}", config.Topic);
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

                    var transactionEvent = JsonSerializer.Deserialize<TransactionEvent>(result.Message.Value);
                    if (transactionEvent == null)
                    {
                        _logger.LogWarning("Failed to deserialize message: {Message}", result.Message.Value);
                        continue;
                    }


                    var scope = _scopeFactory.CreateScope();
                    var bankAccount = await scope.ServiceProvider.GetRequiredService<IBankAccountRepository>()
                        .GetByIdAsync(transactionEvent.BankAccountId);

                    var transaction = new Transaction
                    {
                        Id = transactionEvent.Id,
                        Date = transactionEvent.Date,
                        Amount = transactionEvent.Amount,
                        Currency = transactionEvent.Currency,
                        Comment = transactionEvent.Comment,
                        Type = transactionEvent.Type,
                        Status = transactionEvent.Status,
                        BankAccountId = transactionEvent.BankAccountId,
                        BankAccount = bankAccount,
                        RelatedTransactionId = transactionEvent.RelatedTransactionId
                    };

                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();

                    await transactionRepository.AddAsync(transaction);

                    var transactionDto = new TransactionDto
                    {
                        Id = transaction.Id,
                        Date = transaction.Date,
                        Amount = transaction.Amount,
                        Comment = transaction.Comment,
                        Type = transaction.Type,
                        Status = transaction.Status
                    };

                    //await _transactionHub.SendTransactionUpdate(transactionDto);
                    //await _transactionHub.SendTransactionUpdateToBankAccount(transaction.BankAccountId, transactionDto);

                    var transactionExecutor = scope.ServiceProvider
                        .GetRequiredService<ITransactionExecutor>();

                    await transactionExecutor.ExecuteTransactionAsync(transaction);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message: {Reason}", ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while processing message.");
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
            _logger.LogInformation("Consumer closed.");
        }
    }
}