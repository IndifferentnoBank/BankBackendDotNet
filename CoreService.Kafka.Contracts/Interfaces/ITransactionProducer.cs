using CoreService.Kafka.Contracts.Events;

namespace CoreService.Kafka.Contracts.Interfaces;

public interface ITransactionProducer
{
    Task ProduceTransactionEventAsync(TransactionEvent transactionEvent);
}