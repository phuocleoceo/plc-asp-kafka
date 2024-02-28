using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducerConfig<TK, TV> : ProducerConfig
{
    public string Topic { get; set; }
}
