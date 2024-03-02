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

    public async Task HandleAsync(string key, User value)
    {
        _logger.LogInformation($"Consume event for key: {key} with value: {value}");
        await Task.CompletedTask;
    }
}
