using PlcKafkaConsumer.Models;
using PlcKafkaLibrary.Consumer;

namespace PlcKafkaConsumer.EventHandlers;

public class DrinkHandler(ILogger<DrinkHandler> logger) : IKafkaConsumerHandler<string, Drink>
{
    public string Topic => "Drink";

    public async Task HandleAsync(KafkaConsumeResult<string, Drink> result)
    {
        string topic = result.Topic;
        string key = result.Key;
        Drink drink = result.Value;

        logger.LogInformation(
            $"Consume event from topic: {topic} for key: {key} with value: {drink}"
        );
        await Task.CompletedTask;
    }
}
