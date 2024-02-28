using Microsoft.Extensions.Options;
using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducer<TK, TV> : IDisposable
{
    private readonly IProducer<TK, TV> _producer;
    private readonly string _topic;

    public KafkaProducer(
        IOptions<KafkaProducerConfig<TK, TV>> kafkaProducerConfig,
        IProducer<TK, TV> producer
    )
    {
        _topic = kafkaProducerConfig.Value.Topic;
        _producer = producer;
    }

    public async Task ProduceAsync(TK key, TV value)
    {
        await _producer.ProduceAsync(_topic, new Message<TK, TV> { Key = key, Value = value });
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
