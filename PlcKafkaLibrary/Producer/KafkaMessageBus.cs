namespace PlcKafkaLibrary.Producer;

public class KafkaMessageBus<TKey, TValue>(KafkaProducer<TKey, TValue> producer)
    : IKafkaMessageBus<TKey, TValue>
{
    public async Task PublishAsync(
        string topic,
        TKey key,
        TValue message,
        Dictionary<string, byte[]> headers = null
    )
    {
        await producer.ProduceAsync(topic, key, message, headers);
    }
}
