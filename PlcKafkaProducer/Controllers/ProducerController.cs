using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

using PlcKafkaLibrary.Producer;
using PlcKafkaProducer.Models;

namespace PlcKafkaProducer.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProducerController : ControllerBase
{
    private readonly IKafkaMessageBus<string, User> _kafkaMessageBus;
    private readonly ILogger<ProducerController> _logger;

    public ProducerController(
        IKafkaMessageBus<string, User> kafkaMessageBus,
        ILogger<ProducerController> logger
    )
    {
        _kafkaMessageBus = kafkaMessageBus;
        _logger = logger;
    }

    [HttpPost]
    public async Task SendUser([FromBody] User user)
    {
        await _kafkaMessageBus.PublishAsync(Guid.NewGuid().ToString(), user);
        _logger.LogInformation($"Produce message {user}");
    }
}
