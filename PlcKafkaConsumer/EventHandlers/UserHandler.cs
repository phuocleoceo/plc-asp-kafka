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

    public string Topic => "User";

    public async Task HandleAsync(KafkaConsumeResult<string, User> result)
    {
        string topic = result.Topic;
        string key = result.Key;
        User user = result.Value;

        _logger.LogInformation(
            $"Consume event from topic: {topic} for key: {key} with value: {user}"
        );
        await Task.CompletedTask;
    }
}
