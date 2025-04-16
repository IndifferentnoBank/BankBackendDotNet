using System.Text.Json;
using Common.Configurations;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Domain.Entities;
using CoreService.Contracts.Events;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Enums;
using CoreService.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
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


    public TransactionConsumer(IOptions<KafkaConfiguration> kafkaConfigOptions,
        ITransactionExecutor transactionExecutor, ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository, ILogger<TransactionConsumer> logger,
        ITransactionHub transactionHub)
    {
        _transactionExecutor = transactionExecutor;
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
        _logger = logger;
        _transactionHub = transactionHub;
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