using System.Text;
using PlcKafkaConsumer.Models;
using PlcKafkaLibrary.Consumer;

namespace PlcKafkaConsumer.EventHandlers;

public class UserHandler(ILogger<UserHandler> logger) : IKafkaConsumerHandler<string, User>
{
    public string Topic => "User";

    public async Task HandleAsync(KafkaConsumeResult<string, User> result)
    {
        string topic = result.Topic;
        string key = result.Key;
        User user = result.Value;
        Dictionary<string, byte[]> headers = result.Headers;

        logger.LogInformation(
            $"Consume event from topic: {topic} for key: {key} with value: {user}"
        );

        foreach (var (headerKey, headerValue) in headers)
        {
            logger.LogInformation(
                $"Header key: {headerKey}, value: {Encoding.UTF8.GetString(headerValue)}"
            );
        }

        await Task.CompletedTask;
    }
}
