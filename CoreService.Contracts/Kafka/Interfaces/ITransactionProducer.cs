using CoreService.Contracts.Kafka.Events;

namespace CoreService.Contracts.Kafka.Interfaces;

public interface ITransactionProducer
{
    Task ProduceTransactionEventAsync(TransactionEvent transactionEvent);
}