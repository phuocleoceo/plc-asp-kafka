using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaConsumerConfig : ConsumerConfig
{
    public string Topic { get; set; }
    public double Timeout { get; set; }

    public KafkaConsumerConfig()
    {
        Timeout = 1000;
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        EnableAutoOffsetStore = false;
    }
}
