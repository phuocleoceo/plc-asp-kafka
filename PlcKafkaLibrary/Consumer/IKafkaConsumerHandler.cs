using Confluent.Kafka;

namespace PlcKafkaLibrary.Consumer;

public interface IKafkaConsumerHandler<TKey, TValue>
{
    string Topic { get; }

    Task HandleAsync(ConsumeResult<TKey, TValue> result);
}
