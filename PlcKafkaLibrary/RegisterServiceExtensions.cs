using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Producer;
using PlcKafkaLibrary.Consumer;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // KafkaConfig kafkaConfig = configuration.GetSection("Kafka").Get<KafkaConfig>();
        services.Configure<KafkaConfig>(configuration.GetSection("Kafka"));
        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services
    )
    {
        services.AddSingleton(serviceProvider =>
        {
            KafkaConfig kafkaConfig = serviceProvider
                .GetRequiredService<IOptions<KafkaConfig>>()
                .Value;

            ProducerBuilder<TKey, TValue> builder = new ProducerBuilder<TKey, TValue>(
                kafkaConfig.ProducerConfig()
            ).SetValueSerializer(new KafkaSerializer<TValue>());

            return builder.Build();
        });

        services.AddSingleton<KafkaProducer<TKey, TValue>>();

        services.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(
        this IServiceCollection services
    )
        where THandler : class, IKafkaConsumerHandler<TKey, TValue>
    {
        services.AddScoped<IKafkaConsumerHandler<TKey, TValue>, THandler>();

        services.AddSingleton<IHostedService, KafkaConsumerService<TKey, TValue>>();

        return services;
    }
}
