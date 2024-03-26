using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka.Admin;
using Confluent.Kafka;

using PlcKafkaLibrary.Configuration;

namespace PlcKafkaLibrary.Admin;

public class KafkaCreateTopic
{
    private readonly Dictionary<string, KafkaTopicConfig> _kafkaTopicConfigs;

    private readonly ILogger<KafkaCreateTopic> _logger;
    private readonly IAdminClient _adminClient;

    public KafkaCreateTopic(
        IOptions<KafkaConfig> kafkaConfig,
        IAdminClient adminClient,
        ILogger<KafkaCreateTopic> logger
    )
    {
        _logger = logger;
        _adminClient = adminClient;
        _kafkaTopicConfigs = kafkaConfig.Value.Topic;
    }

    public async Task CreateTopic()
    {
        try
        {
            List<TopicSpecification> topicSpecifications = _kafkaTopicConfigs
                .Select(kafkaTopicConfig =>
                {
                    KafkaTopicConfig topicConfig = kafkaTopicConfig.Value;
                    return new TopicSpecification()
                    {
                        Name = topicConfig.Name,
                        ReplicationFactor = (short)topicConfig.ReplicationFactor,
                        NumPartitions = topicConfig.NumPartitions
                    };
                })
                .ToList();

            await _adminClient.CreateTopicsAsync(topicSpecifications);

            _logger.LogInformation("Created topic");
        }
        catch (Exception)
        {
            _logger.LogInformation("Ignored create topic");
        }
    }
}
