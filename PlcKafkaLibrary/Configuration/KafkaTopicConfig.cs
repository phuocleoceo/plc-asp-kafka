namespace PlcKafkaLibrary.Configuration;

public class KafkaTopicConfig
{
    public string Name { get; set; }
    public int NumPartitions { get; set; } = 1;
    public int ReplicationFactor { get; set; } = 1;
    public int ConsumerRetries { get; set; } = 3;
    public int ConsumerRetryDelayInMs { get; set; } = 7200000; // 2 hours
    public int ConsumerRetryDelayMaxMs { get; set; } = 86400000; // 1 day
    public int Multiplier { get; set; } = 2;
    public List<string> AllowConsumeHeaders { get; set; }
}
