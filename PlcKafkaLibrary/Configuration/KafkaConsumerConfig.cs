using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaConsumerConfig : ConsumerConfig
{
    public double Timeout { get; set; } = 1000; // 1 seconds

    public KafkaConsumerConfig()
    {
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        EnableAutoOffsetStore = false;
    }
}
