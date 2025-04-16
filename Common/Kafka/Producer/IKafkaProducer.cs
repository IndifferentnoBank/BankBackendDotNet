namespace Common.Kafka.Producer;

public interface IKafkaProducer<T> where T: class
{
    Task ProduceAsync(string topic, string key, T message, CancellationToken cancellationToken = default);
}