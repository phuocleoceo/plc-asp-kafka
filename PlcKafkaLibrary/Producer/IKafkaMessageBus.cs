namespace PlcKafkaLibrary.Producer;

public interface IKafkaMessageBus<TKey, TValue>
{
    Task PublishAsync(string topic, TKey key, TValue message);
}
