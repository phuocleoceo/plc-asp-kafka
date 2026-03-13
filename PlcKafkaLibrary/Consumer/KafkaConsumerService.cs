using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary.Consumer;

public class KafkaConsumerService<TKey, TValue>(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<KafkaConfig> kafkaConfig
) : IHostedService
{
    private readonly Dictionary<string, KafkaTopicConfig> _kafkaTopicConfigs = kafkaConfig
        .Value
        .Topic;
    private readonly KafkaConsumerConfig _kafkaConsumerConfig = kafkaConfig.Value.ConsumerConfig;
    private IKafkaConsumerHandler<TKey, TValue> _kafkaConsumerHandler;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        _kafkaConsumerHandler = scope.ServiceProvider.GetRequiredService<
            IKafkaConsumerHandler<TKey, TValue>
        >();

        KafkaTopicConfig kafkaTopicConfig = _kafkaTopicConfigs[_kafkaConsumerHandler.Topic];
        if (kafkaTopicConfig == null || string.IsNullOrWhiteSpace(kafkaTopicConfig.Name))
        {
            return;
        }

        ConsumerBuilder<TKey, TValue> builder = new ConsumerBuilder<TKey, TValue>(
            _kafkaConsumerConfig
        ).SetValueDeserializer(new KafkaDeserializer<TValue>());

        using IConsumer<TKey, TValue> consumer = builder.Build();
        consumer.Subscribe(kafkaTopicConfig.Name);

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<TKey, TValue> result = consumer.Consume(
                TimeSpan.FromMilliseconds(_kafkaConsumerConfig.Timeout)
            );

            if (result == null)
            {
                continue;
            }

            await _kafkaConsumerHandler.HandleAsync(new KafkaConsumeResult<TKey, TValue>(result));
            consumer.Commit(result);
            consumer.StoreOffset(result);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
