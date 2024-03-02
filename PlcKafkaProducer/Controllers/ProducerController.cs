using Microsoft.AspNetCore.Mvc;

using PlcKafkaLibrary.Producer;
using PlcKafkaProducer.Models;

namespace PlcKafkaProducer.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProducerController : ControllerBase
{
    private readonly IKafkaMessageBus<string, object> _kafkaMessageBus;
    private readonly ILogger<ProducerController> _logger;

    public ProducerController(
        IKafkaMessageBus<string, object> kafkaMessageBus,
        ILogger<ProducerController> logger
    )
    {
        _kafkaMessageBus = kafkaMessageBus;
        _logger = logger;
    }

    [HttpPost]
    public async Task SendUser([FromBody] User user)
    {
        string topic = "plc-users";
        string key = Guid.NewGuid().ToString();
        await _kafkaMessageBus.PublishAsync(topic, key, user);
        _logger.LogInformation($"Produce message {user} with key: {key} to topic: {topic}");
    }
}
