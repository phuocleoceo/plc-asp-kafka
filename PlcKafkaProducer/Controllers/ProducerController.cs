using System.Text;
using Microsoft.AspNetCore.Mvc;
using PlcKafkaLibrary.Producer;
using PlcKafkaProducer.Models;

namespace PlcKafkaProducer.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProducerController(
    IKafkaMessageBus<string, object> kafkaMessageBus,
    ILogger<ProducerController> logger
) : ControllerBase
{
    [HttpPost("user")]
    public async Task SendUser([FromBody] User user)
    {
        string topic = "User";
        string key = Guid.NewGuid().ToString();
        Dictionary<string, byte[]> headers = new() { { "event", Encoding.UTF8.GetBytes("user") } };

        await kafkaMessageBus.PublishAsync(topic, key, user, headers);
        logger.LogInformation($"Produce message {user} with key: {key} to topic: {topic}");
    }

    [HttpPost("drink")]
    public async Task SendDrink([FromBody] Drink drink)
    {
        string topic = "Drink";
        string key = Guid.NewGuid().ToString();

        await kafkaMessageBus.PublishAsync(topic, key, drink);
        logger.LogInformation($"Produce message {drink} with key: {key} to topic: {topic}");
    }
}
