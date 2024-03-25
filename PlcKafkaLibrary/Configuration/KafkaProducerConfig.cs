using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaProducerConfig : ProducerConfig
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
