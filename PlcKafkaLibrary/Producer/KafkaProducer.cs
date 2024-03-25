using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducer<TKey, TValue> : IDisposable
{
    private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;
    private readonly KafkaProducerConfig _kafkaProducerConfig;
    private readonly IProducer<TKey, TValue> _producer;

    public KafkaProducer(
        IOptions<KafkaConfig> kafkaConfig,
        ILogger<KafkaProducer<TKey, TValue>> logger
    )
    {
        _logger = logger;
        _kafkaProducerConfig = kafkaConfig.Value.ProducerConfig;
        _producer = new ProducerBuilder<TKey, TValue>(_kafkaProducerConfig)
            .SetValueSerializer(new KafkaSerializer<TValue>())
            .Build();
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
