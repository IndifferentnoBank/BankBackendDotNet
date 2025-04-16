using CoreService.Contracts.Events;

namespace CoreService.Contracts.Interfaces;

public interface ITransactionProducer
{
    Task ProduceTransactionEventAsync(TransactionEvent transactionEvent);
}