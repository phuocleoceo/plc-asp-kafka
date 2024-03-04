using Confluent.Kafka;

namespace PlcKafkaLibrary.Producer;

public class KafkaProducerConfig<TKey, TValue> : ProducerConfig
{
    public KafkaProducerConfig()
    {
        Acks = Confluent.Kafka.Acks.All;
        EnableIdempotence = true;
        MaxInFlight = 5;
        RetryBackoffMs = 100;
        MessageSendMaxRetries = int.MaxValue;
        CompressionType = Confluent.Kafka.CompressionType.Gzip;
    }
}
