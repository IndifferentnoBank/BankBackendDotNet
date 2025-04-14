using System.Text.Json;
using Common.Configurations;
using Common.Kafka.Consumer;
using Confluent.Kafka;
using CoreService.Application.Helpers.TransactionExecutor;
using CoreService.Domain.Entities;
using CoreService.Contracts.Events;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using CoreService.Persistence.Repositories.BankAccountRepository;
using CoreService.Persistence.Repositories.TransactionsRepository;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Consumers;

public class TransactionConsumer : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ITransactionExecutor _transactionExecutor;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public TransactionConsumer(IOptions<KafkaConfiguration> kafkaConfigOptions,
        ITransactionExecutor transactionExecutor, ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository)
    {
        _transactionExecutor = transactionExecutor;
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
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
                await _transactionExecutor.ExecuteTransactionAsync(transactionToProcess);
            }
        }, cancellationToken);
    }
}