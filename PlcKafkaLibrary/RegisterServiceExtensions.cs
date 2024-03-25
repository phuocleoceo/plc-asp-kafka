using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Consumer;
using PlcKafkaLibrary.Producer;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        KafkaConfig kafkaConfig = configuration.GetSection("Kafka").Get<KafkaConfig>();
        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services,
        Action<KafkaProducerConfig> configAction
    )
    {
        services.Configure(configAction);

        services.AddSingleton(serviceProvider =>
        {
            IOptions<KafkaProducerConfig> config = serviceProvider.GetRequiredService<
                IOptions<KafkaProducerConfig>
            >();

            ProducerBuilder<TKey, TValue> builder = new ProducerBuilder<TKey, TValue>(
                config.Value
            ).SetValueSerializer(new KafkaSerializer<TValue>());

            return builder.Build();
        });

        services.AddSingleton<KafkaProducer<TKey, TValue>>();

        services.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(
        this IServiceCollection services,
        Action<KafkaConsumerConfig> configAction
    )
        where THandler : class, IKafkaConsumerHandler<TKey, TValue>
    {
        services.Configure(configAction);

        services.AddScoped<IKafkaConsumerHandler<TKey, TValue>, THandler>();

        services.AddSingleton<IHostedService, KafkaConsumerService<TKey, TValue>>();

        return services;
    }
}
