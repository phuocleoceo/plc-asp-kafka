using Confluent.Kafka;

namespace PlcKafkaLibrary.Consumer;

public class KafkaConsumerConfig<TKey, TValue> : ConsumerConfig
{
    public string Topic { get; set; }

    public KafkaConsumerConfig()
    {
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        EnableAutoOffsetStore = false;
    }
}
