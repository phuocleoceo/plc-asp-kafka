using Confluent.Kafka;

namespace PlcKafkaLibrary.Consumer;

public interface IKafkaConsumerHandler<TKey, TValue>
{
    Task HandleAsync(ConsumeResult<TKey, TValue> result);
}
