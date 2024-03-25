using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary.Consumer;

public class KafkaConsumerService<TKey, TValue> : IHostedService
{
    private readonly KafkaConsumerConfig _kafkaConsumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger;
    private IKafkaConsumerHandler<TKey, TValue> _kafkaConsumerHandler;

    public KafkaConsumerService(
        ILogger<KafkaConsumerService<TKey, TValue>> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<KafkaConfig> kafkaConfig
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _kafkaConsumerConfig = kafkaConfig.Value.ConsumerConfig;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        _kafkaConsumerHandler = scope.ServiceProvider.GetRequiredService<
            IKafkaConsumerHandler<TKey, TValue>
        >();

        ConsumerBuilder<TKey, TValue> builder = new ConsumerBuilder<TKey, TValue>(
            _kafkaConsumerConfig
        ).SetValueDeserializer(new KafkaDeserializer<TValue>());

        using IConsumer<TKey, TValue> consumer = builder.Build();
        consumer.Subscribe(_kafkaConsumerHandler.Topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<TKey, TValue> result = consumer.Consume(
                TimeSpan.FromMilliseconds(_kafkaConsumerConfig.Timeout)
            );

            if (result == null)
            {
                continue;
            }

            await _kafkaConsumerHandler.HandleAsync(result);
            consumer.Commit(result);
            consumer.StoreOffset(result);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
