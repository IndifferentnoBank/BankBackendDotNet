using Common.Configurations;
using Common.Kafka.Producer;
using CoreService.Contracts.Events;
using CoreService.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Producers;

public class TransactionProducer : ITransactionProducer
{
    private readonly IKafkaProducer<TransactionEvent> _kafkaProducer;
    private readonly ILogger<TransactionProducer> _logger;
    private readonly string _topic;

    public TransactionProducer(IOptions<KafkaConfiguration> kafkaConfiguration,
        IKafkaProducer<TransactionEvent> kafkaProducer, ILogger<TransactionProducer> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
        _topic = kafkaConfiguration.Value.Topic;
    }

    public async Task ProduceTransactionEventAsync(TransactionEvent transactionEvent)
    {
        var key = transactionEvent.Id.ToString();
        await _kafkaProducer.ProduceAsync(_topic, key, transactionEvent);
        _logger.LogInformation("Transaction event produced: {id}", transactionEvent.Id);
    }
}