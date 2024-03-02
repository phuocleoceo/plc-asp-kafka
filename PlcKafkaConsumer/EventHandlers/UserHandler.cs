using Confluent.Kafka;
using PlcKafkaLibrary.Consumer;
using PlcKafkaProducer.Models;

namespace PlcKafkaConsumer.EventHandlers;

public class UserHandler : IKafkaConsumerHandler<string, User>
{
    private readonly ILogger<UserHandler> _logger;

    public UserHandler(ILogger<UserHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(ConsumeResult<string, User> result)
    {
        string topic = result.Topic;
        string key = result.Message.Key;
        User user = result.Message.Value;

        _logger.LogInformation(
            $"Consume event from topic: {topic} for key: {key} with value: {user}"
        );
        await Task.CompletedTask;
    }
}
