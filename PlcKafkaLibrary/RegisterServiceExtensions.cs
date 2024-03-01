using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Consumer;
using PlcKafkaLibrary.Producer;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services,
        Action<KafkaProducerConfig<TKey, TValue>> configAction
    )
    {
        services.AddSingleton(serviceProvider =>
        {
            IOptions<KafkaProducerConfig<TKey, TValue>> config = serviceProvider.GetRequiredService<
                IOptions<KafkaProducerConfig<TKey, TValue>>
            >();

            ProducerBuilder<TKey, TValue> builder = new ProducerBuilder<TKey, TValue>(
                config.Value
            ).SetValueSerializer(new KafkaSerializer<TValue>());

            return builder.Build();
        });

        services.AddSingleton<KafkaProducer<TKey, TValue>>();

        services.Configure(configAction);

        services.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(
        this IServiceCollection services,
        Action<KafkaConsumerConfig<TKey, TValue>> configAction
    )
        where THandler : class, IKafkaConsumerHandler<TKey, TValue>
    {
        services.AddScoped<IKafkaConsumerHandler<TKey, TValue>, THandler>();

        services.AddHostedService<BackGroundKafkaConsumer<TKey, TValue>>();

        services.Configure(configAction);

        return services;
    }
}
