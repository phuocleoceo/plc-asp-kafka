using Confluent.Kafka;
using PlcKafkaLibrary.Consumer;
using PlcKafkaProducer.Models;

namespace PlcKafkaConsumer.EventHandlers;

public class DrinkHandler : IKafkaConsumerHandler<string, Drink>
{
    private readonly ILogger<DrinkHandler> _logger;

    public DrinkHandler(ILogger<DrinkHandler> logger)
    {
        _logger = logger;
    }

    public string Topic => "Drink";

    public async Task HandleAsync(ConsumeResult<string, Drink> result)
    {
        string topic = result.Topic;
        string key = result.Message.Key;
        Drink drink = result.Message.Value;

        _logger.LogInformation(
            $"Consume event from topic: {topic} for key: {key} with value: {drink}"
        );
        await Task.CompletedTask;
    }
}
