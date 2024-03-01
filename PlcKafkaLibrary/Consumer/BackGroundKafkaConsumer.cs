using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary.Consumer;

public class BackGroundKafkaConsumer<TKey, TValue> : BackgroundService
{
    private readonly KafkaConsumerConfig<TKey, TValue> _kafkaConsumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IKafkaConsumerHandler<TKey, TValue> _kafkaConsumerHandler;

    public BackGroundKafkaConsumer(
        IOptions<KafkaConsumerConfig<TKey, TValue>> kafkaConsumerConfig,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _kafkaConsumerConfig = kafkaConsumerConfig.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        _kafkaConsumerHandler = scope.ServiceProvider.GetRequiredService<
            IKafkaConsumerHandler<TKey, TValue>
        >();

        ConsumerBuilder<TKey, TValue> builder = new ConsumerBuilder<TKey, TValue>(
            _kafkaConsumerConfig
        ).SetValueDeserializer(new KafkaDeserializer<TValue>());

        using IConsumer<TKey, TValue> consumer = builder.Build();
        consumer.Subscribe(_kafkaConsumerConfig.Topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<TKey, TValue> result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

            if (result == null)
            {
                continue;
            }

            await _kafkaConsumerHandler.HandleAsync(result.Message.Key, result.Message.Value);
            consumer.Commit(result);
            consumer.StoreOffset(result);
        }
    }
}
