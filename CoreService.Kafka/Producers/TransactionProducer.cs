using Common.Kafka.Configs;
using Common.Kafka.Producer;
using CoreService.Contracts.Kafka.Events;
using CoreService.Contracts.Kafka.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Producers;

public class TransactionProducer : ITransactionProducer
{
    private readonly IKafkaProducer<TransactionEvent> _kafkaProducer;
    private readonly ILogger<TransactionProducer> _logger;
    private readonly string _topic;

    public TransactionProducer(
        IOptions<KafkaProducerConfiguration> kafkaConfiguration,
        IKafkaProducer<TransactionEvent> kafkaProducer,
        ILogger<TransactionProducer> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
        _topic = kafkaConfiguration.Value.Topic;
    }

    public async Task ProduceTransactionEventAsync(TransactionEvent transactionEvent)
    {
        try
        {
            var key = transactionEvent.Id.ToString();
            await _kafkaProducer.ProduceAsync(_topic, key, transactionEvent);
            _logger.LogInformation("Transaction event produced: {id}", transactionEvent.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to produce transaction event: {id}", transactionEvent.Id);
            throw; 
        }
    }
}