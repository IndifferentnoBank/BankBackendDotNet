using Common.Configurations;
using Common.Kafka.Producer;
using CoreService.Kafka.Contracts.Events;
using CoreService.Kafka.Contracts.Interfaces;
using Microsoft.Extensions.Options;

namespace CoreService.Kafka.Producers;

public class TransactionProducer : ITransactionProducer
{
    private readonly IKafkaProducer<TransactionEvent> _kafkaProducer;
    private readonly string _topic;

    public TransactionProducer(IOptions<KafkaConfiguration> kafkaConfiguration,
        IKafkaProducer<TransactionEvent> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
        _topic = kafkaConfiguration.Value.Topic;
    }

    public async Task ProduceTransactionEventAsync(TransactionEvent transactionEvent)
    {
        var key = transactionEvent.Id.ToString();
        await _kafkaProducer.ProduceAsync(_topic, key, transactionEvent);
    }
}