using Confluent.Kafka;

namespace PlcKafkaLibrary.Configuration;

public class KafkaAdminClientConfig : AdminClientConfig
{
    public bool CreateTopic { get; set; } = true;
    public List<string> LoggingMetadata { get; set; } = new();
}
