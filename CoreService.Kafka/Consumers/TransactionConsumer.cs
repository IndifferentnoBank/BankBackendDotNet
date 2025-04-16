using System.Text.Json;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Domain.Entities;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Repositories;
using CoreService.Kafka.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Consumers;

public class TransactionConsumer : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ITransactionExecutor _transactionExecutor;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ILogger<TransactionConsumer> _logger;
    private readonly ITransactionHub _transactionHub;


    public TransactionConsumer(
        IOptions<BankTransactionsConsumerConfig> configOptions,
        ITransactionExecutor executor,
        ITransactionRepository trRepo,
        IBankAccountRepository accRepo,
        ILogger<TransactionConsumer> logger,
        ITransactionHub hub)
    {
        var config = configOptions.Value;
        _transactionExecutor = executor;
        _transactionRepository = trRepo;
        _bankAccountRepository = accRepo;
        _logger = logger;
        _transactionHub = hub;

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
                var transaction = JsonSerializer.Deserialize<TransactionEvent>(result.Message.Value);

                var bankAccount = await _bankAccountRepository.GetByIdAsync(transaction.BankAccountId);
                var transactionToProcess = new Transaction
                {
                    Id = transaction.Id,
                    Date = transaction.Date,
                    Amount = transaction.Amount,
                    Currency = transaction.Currency,
                    Comment = transaction.Comment,
                    Type = transaction.Type,
                    Status = transaction.Status,
                    BankAccountId = transaction.BankAccountId,
                    BankAccount = bankAccount,
                    RelatedTransactionId = transaction.RelatedTransactionId
                };

                await _transactionRepository.AddAsync(transactionToProcess);

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

                await _transactionExecutor.ExecuteTransactionAsync(transactionToProcess);
            }
        }, cancellationToken);
    }
}