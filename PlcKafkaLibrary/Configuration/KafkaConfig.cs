using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaConfig
{
    public string BootstrapServers { get; set; }
    public SaslMechanism SaslMechanism { get; set; }
    public SecurityProtocol SecurityProtocol { get; set; }
    public string SaslUsername { get; set; }
    public string SaslPassword { get; set; }
    public KafkaProducerConfig Producer { get; set; } = new();
    public KafkaConsumerConfig Consumer { get; set; } = new();
    public List<KafkaTopicConfig> Topics { get; set; } = new();
}
