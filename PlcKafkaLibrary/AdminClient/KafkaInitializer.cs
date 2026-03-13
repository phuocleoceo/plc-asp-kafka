using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlcKafkaLibrary.Configuration;

namespace PlcKafkaLibrary.AdminClient;

public class KafkaInitializer(
    ILogger<KafkaInitializer> logger,
    IOptions<KafkaConfig> kafkaConfig,
    IAdminClient adminClient
)
{
    private readonly Dictionary<string, KafkaTopicConfig> _kafkaTopicConfigs = kafkaConfig
        .Value
        .Topic;
    private readonly KafkaAdminClientConfig _kafkaAdminClientConfig = kafkaConfig
        .Value
        .AdminClientConfig;

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
            List<TopicSpecification> topicSpecifications =
            [
                .. _kafkaTopicConfigs.Select(kafkaTopicConfig =>
                {
                    KafkaTopicConfig topicConfig = kafkaTopicConfig.Value;
                    return new TopicSpecification()
                    {
                        Name = topicConfig.Name,
                        ReplicationFactor = (short)topicConfig.ReplicationFactor,
                        NumPartitions = topicConfig.NumPartitions,
                    };
                }),
            ];

            await adminClient.CreateTopicsAsync(topicSpecifications);

            logger.LogInformation("Created topic");
        }
        catch (Exception)
        {
            logger.LogInformation("Ignored create topic");
        }
    }

    private void PrintMetadata()
    {
        List<string> loggingMetadata = _kafkaAdminClientConfig.LoggingMetadata;

        if (loggingMetadata.Count == 0)
        {
            return;
        }

        Metadata metadata = adminClient.GetMetadata(TimeSpan.FromMilliseconds(20000));

        if (loggingMetadata.Contains("Broker"))
        {
            StringBuilder kafkaBroker = new();
            kafkaBroker.AppendLine("**** Kafka broker ****");
            kafkaBroker.AppendLine($"- OriginatingBrokerId: {metadata.OriginatingBrokerId}");
            kafkaBroker.AppendLine($"- OriginatingBrokerName: {metadata.OriginatingBrokerName}");
            metadata.Brokers.ForEach(broker =>
            {
                kafkaBroker.AppendLine($"- BrokerId: {broker.BrokerId}");
                kafkaBroker.AppendLine($"- Broker Host: {broker.Host}:{broker.Port}");
            });
            logger.LogInformation(kafkaBroker.ToString());
        }

        if (loggingMetadata.Contains("Topic"))
        {
            StringBuilder kafkaTopic = new();
            kafkaTopic.AppendLine("**** Kafka topic ****");
            metadata.Topics.ForEach(topic =>
            {
                kafkaTopic.AppendLine($"- Topic: {topic.Topic}");
                kafkaTopic.AppendLine($"  + Status: {topic.Error}");
                kafkaTopic.AppendLine($"  + NumPartitions: {topic.Partitions.Count}");
            });
            logger.LogInformation(kafkaTopic.ToString());
        }
    }
}
