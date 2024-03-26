namespace PlcKafkaLibrary.Producer;

public interface IKafkaMessageBus<TKey, TValue>
{
    Task PublishAsync(
        string topic,
        TKey key,
        TValue message,
        Dictionary<string, byte[]> headers = null
    );
}
