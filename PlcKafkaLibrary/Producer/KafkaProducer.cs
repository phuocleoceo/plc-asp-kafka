using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducer<TKey, TValue> : IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;

    public KafkaProducer(IProducer<TKey, TValue> producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync(string topic, TKey key, TValue value)
    {
        await _producer.ProduceAsync(topic, new Message<TKey, TValue> { Key = key, Value = value });
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
