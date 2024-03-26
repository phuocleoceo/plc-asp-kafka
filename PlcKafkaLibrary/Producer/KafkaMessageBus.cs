namespace PlcKafkaLibrary.Producer;

public class KafkaMessageBus<TKey, TValue> : IKafkaMessageBus<TKey, TValue>
{
    private readonly KafkaProducer<TKey, TValue> _producer;

    public KafkaMessageBus(KafkaProducer<TKey, TValue> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(
        string topic,
        TKey key,
        TValue message,
        Dictionary<string, byte[]> headers = null
    )
    {
        await _producer.ProduceAsync(topic, key, message, headers);
    }
}
