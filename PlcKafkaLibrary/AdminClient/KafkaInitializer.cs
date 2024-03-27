using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

using Confluent.Kafka.Admin;
using Confluent.Kafka;

using PlcKafkaLibrary.Configuration;

namespace PlcKafkaLibrary.AdminClient;

public class KafkaInitializer
{
    private readonly Dictionary<string, KafkaTopicConfig> _kafkaTopicConfigs;
    private readonly KafkaAdminClientConfig _kafkaAdminClientConfig;

    private readonly ILogger<KafkaInitializer> _logger;
    private readonly IAdminClient _adminClient;

    public KafkaInitializer(
        ILogger<KafkaInitializer> logger,
        IOptions<KafkaConfig> kafkaConfig,
        IAdminClient adminClient
    )
    {
        _logger = logger;
        _adminClient = adminClient;
        _kafkaTopicConfigs = kafkaConfig.Value.Topic;
        _kafkaAdminClientConfig = kafkaConfig.Value.AdminClientConfig;
    }

    public async Task Initialize()
    {
        try
        {
            await CreateTopic();
            PrintMetadata();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private async Task CreateTopic()
    {
        if (!_kafkaAdminClientConfig.CreateTopic)
        {
            return;
        }

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

    private void PrintMetadata()
    {
        List<string> loggingMetadata = _kafkaAdminClientConfig.LoggingMetadata;

        if (loggingMetadata.Count == 0)
        {
            return;
        }

        Metadata metadata = _adminClient.GetMetadata(TimeSpan.FromMilliseconds(20000));

        if (loggingMetadata.Contains("Broker"))
        {
            StringBuilder kafkaBroker = new StringBuilder();
            kafkaBroker.AppendLine("**** Kafka broker ****");
            kafkaBroker.AppendLine($"- OriginatingBrokerId: {metadata.OriginatingBrokerId}");
            kafkaBroker.AppendLine($"- OriginatingBrokerName: {metadata.OriginatingBrokerName}");
            metadata.Brokers.ForEach(broker =>
            {
                kafkaBroker.AppendLine($"- BrokerId: {broker.BrokerId}");
                kafkaBroker.AppendLine($"- Broker Host: {broker.Host}:{broker.Port}");
            });
            _logger.LogInformation(kafkaBroker.ToString());
        }

        if (loggingMetadata.Contains("Topic"))
        {
            StringBuilder kafkaTopic = new StringBuilder();
            kafkaTopic.AppendLine("**** Kafka topic ****");
            metadata.Topics.ForEach(topic =>
            {
                kafkaTopic.AppendLine($"- Topic: {topic.Topic}");
                kafkaTopic.AppendLine($"  + Status: {topic.Error}");
                kafkaTopic.AppendLine($"  + NumPartitions: {topic.Partitions.Count}");
            });
            _logger.LogInformation(kafkaTopic.ToString());
        }
    }
}
