namespace PlcKafkaLibrary.Producer;

public interface IKafkaMessageBus<TKey, TValue>
{
    Task PublishAsync(TKey key, TValue message);
}
