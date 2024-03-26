using System.Text;
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
        Dictionary<string, byte[]> headers = result.Headers;

        _logger.LogInformation(
            $"Consume event from topic: {topic} for key: {key} with value: {user}"
        );

        foreach (var (headerKey, headerValue) in headers)
        {
            _logger.LogInformation(
                $"Header key: {headerKey}, value: {Encoding.UTF8.GetString(headerValue)}"
            );
        }

        await Task.CompletedTask;
    }
}
