using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary.Consumer;

public class BackGroundKafkaConsumer<TK, TV> : BackgroundService
{
    private readonly KafkaConsumerConfig<TK, TV> _kafkaConsumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IKafkaHandler<TK, TV> _handler;

    public BackGroundKafkaConsumer(
        IOptions<KafkaConsumerConfig<TK, TV>> kafkaConsumerConfig,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _kafkaConsumerConfig = kafkaConsumerConfig.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        _handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<TK, TV>>();

        ConsumerBuilder<TK, TV> builder = new ConsumerBuilder<TK, TV>(
            _kafkaConsumerConfig
        ).SetValueDeserializer(new KafkaDeserializer<TV>());

        using IConsumer<TK, TV> consumer = builder.Build();
        consumer.Subscribe(_kafkaConsumerConfig.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            ConsumeResult<TK, TV> result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

            if (result == null)
            {
                continue;
            }

            await _handler.HandleAsync(result.Message.Key, result.Message.Value);
            consumer.Commit(result);
            consumer.StoreOffset(result);
        }
    }
}
