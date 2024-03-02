using Microsoft.Extensions.Options;
using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducer<TKey, TValue> : IDisposable
{
    private readonly KafkaProducerConfig<TKey, TValue> _kafkaProducerConfig;
    private readonly IProducer<TKey, TValue> _producer;

    public KafkaProducer(
        IOptions<KafkaProducerConfig<TKey, TValue>> kafkaProducerConfig,
        IProducer<TKey, TValue> producer
    )
    {
        _kafkaProducerConfig = kafkaProducerConfig.Value;
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
