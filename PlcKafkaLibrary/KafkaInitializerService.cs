using Microsoft.Extensions.Hosting;
using PlcKafkaLibrary.AdminClient;

namespace PlcKafkaLibrary;

public class KafkaInitializerService(KafkaInitializer kafkaInitializer) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await kafkaInitializer.Initialize();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
