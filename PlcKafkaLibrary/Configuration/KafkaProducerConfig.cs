using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaProducerConfig : ProducerConfig
{
    public int DeliveryTimeoutMs { get; set; } = 120000; // 2 minutes

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
