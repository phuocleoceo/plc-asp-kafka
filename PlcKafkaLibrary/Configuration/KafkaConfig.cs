using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaConfig
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public SaslMechanism SaslMechanism { get; set; } = SaslMechanism.Plain;
    public SecurityProtocol SecurityProtocol { get; set; }
    public string SaslUsername { get; set; }
    public string SaslPassword { get; set; }
    public KafkaProducerConfig Producer { get; set; } = new();
    public KafkaConsumerConfig Consumer { get; set; } = new();
    public Dictionary<string, KafkaTopicConfig> Topics { get; set; } = new();

    public KafkaProducerConfig ProducerConfig
    {
        get
        {
            Producer.BootstrapServers = BootstrapServers;
            Producer.SaslMechanism = SaslMechanism;
            Producer.SecurityProtocol = SecurityProtocol;
            Producer.SaslUsername = SaslUsername;
            Producer.SaslPassword = SaslPassword;
            return Producer;
        }
    }

    public KafkaConsumerConfig ConsumerConfig
    {
        get
        {
            Consumer.BootstrapServers = BootstrapServers;
            Consumer.SaslMechanism = SaslMechanism;
            Consumer.SecurityProtocol = SecurityProtocol;
            Consumer.SaslUsername = SaslUsername;
            Consumer.SaslPassword = SaslPassword;
            return Consumer;
        }
    }
}
