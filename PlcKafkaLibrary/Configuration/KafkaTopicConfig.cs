namespace PlcKafkaLibrary.Configuration;

public class KafkaTopicConfig
{
    public string Name { get; set; }
    public int NumPartitions { get; set; } = 1;
    public int ReplicationFactor { get; set; } = 1;
}
