using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaConfig
{
    public List<string> BootstrapServers { get; set; } = new() { "localhost:9092" };
    public SaslMechanism SaslMechanism { get; set; } = SaslMechanism.Plain;
    public SecurityProtocol SecurityProtocol { get; set; }
    public string SaslUsername { get; set; }
    public string SaslPassword { get; set; }
    public KafkaAdminClientConfig AdminClient { get; set; } = new();
    public KafkaProducerConfig Producer { get; set; } = new();
    public KafkaConsumerConfig Consumer { get; set; } = new();
    public Dictionary<string, KafkaTopicConfig> Topic { get; set; } = new();

    private string BootstrapServerStrings => string.Join(",", BootstrapServers);

    public KafkaAdminClientConfig AdminClientConfig
    {
        get
        {
            AdminClient.BootstrapServers = BootstrapServerStrings;
            AdminClient.SaslMechanism = SaslMechanism;
            AdminClient.SecurityProtocol = SecurityProtocol;
            AdminClient.SaslUsername = SaslUsername;
            AdminClient.SaslPassword = SaslPassword;
            return AdminClient;
        }
    }

    public KafkaProducerConfig ProducerConfig
    {
        get
        {
            Producer.BootstrapServers = BootstrapServerStrings;
            Producer.SaslMechanism = SaslMechanism;
            Producer.SecurityProtocol = SecurityProtocol;
            Producer.SaslUsername = SaslUsername;
            Producer.SaslPassword = SaslPassword;
            Producer.MessageTimeoutMs = Producer.DeliveryTimeoutMs;
            return Producer;
        }
    }

    public KafkaConsumerConfig ConsumerConfig
    {
        get
        {
            Consumer.BootstrapServers = BootstrapServerStrings;
            Consumer.SaslMechanism = SaslMechanism;
            Consumer.SecurityProtocol = SecurityProtocol;
            Consumer.SaslUsername = SaslUsername;
            Consumer.SaslPassword = SaslPassword;
            return Consumer;
        }
    }
}
