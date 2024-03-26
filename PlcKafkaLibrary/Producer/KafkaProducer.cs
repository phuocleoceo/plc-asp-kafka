using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducer<TKey, TValue> : IDisposable
{
    private readonly Dictionary<string, KafkaTopicConfig> _kafkaTopicConfigs;
    private readonly KafkaProducerConfig _kafkaProducerConfig;

    private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;
    private readonly IProducer<TKey, TValue> _producer;

    public KafkaProducer(
        IOptions<KafkaConfig> kafkaConfig,
        ILogger<KafkaProducer<TKey, TValue>> logger
    )
    {
        _logger = logger;
        _kafkaTopicConfigs = kafkaConfig.Value.Topic;
        _kafkaProducerConfig = kafkaConfig.Value.ProducerConfig;

        _producer = new ProducerBuilder<TKey, TValue>(_kafkaProducerConfig)
            .SetValueSerializer(new KafkaSerializer<TValue>())
            .Build();
    }

    public async Task ProduceAsync(
        string topic,
        TKey key,
        TValue value,
        Dictionary<string, byte[]> headers = null
    )
    {
        KafkaTopicConfig kafkaTopicConfig = _kafkaTopicConfigs[topic];

        if (kafkaTopicConfig == null || string.IsNullOrWhiteSpace(kafkaTopicConfig.Name))
        {
            return;
        }

        Headers kafkaHeaders = new Headers();

        if (headers != null)
        {
            foreach (var (headerKey, headerValue) in headers)
            {
                kafkaHeaders.Add(headerKey, headerValue);
            }
        }

        await _producer.ProduceAsync(
            kafkaTopicConfig.Name,
            new Message<TKey, TValue>
            {
                Key = key,
                Value = value,
                Headers = kafkaHeaders
            }
        );
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
