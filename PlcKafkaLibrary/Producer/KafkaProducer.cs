using Microsoft.Extensions.Options;
using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducer<TKey, TValue> : IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly string _topic;

    public KafkaProducer(
        IOptions<KafkaProducerConfig<TKey, TValue>> kafkaProducerConfig,
        IProducer<TKey, TValue> producer
    )
    {
        _topic = kafkaProducerConfig.Value.Topic;
        _producer = producer;
    }

    public async Task ProduceAsync(TKey key, TValue value)
    {
        await _producer.ProduceAsync(
            _topic,
            new Message<TKey, TValue> { Key = key, Value = value }
        );
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
