using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducerConfig<TKey, TValue> : ProducerConfig
{
    public string Topic { get; set; }
}
