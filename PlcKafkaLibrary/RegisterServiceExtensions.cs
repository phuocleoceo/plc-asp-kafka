using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Consumer;
using PlcKafkaLibrary.Producer;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaProducer<TK, TV>(
        this IServiceCollection services,
        Action<KafkaProducerConfig<TK, TV>> configAction
    )
    {
        services.AddSingleton(serviceProvider =>
        {
            IOptions<KafkaProducerConfig<TK, TV>> config = serviceProvider.GetRequiredService<
                IOptions<KafkaProducerConfig<TK, TV>>
            >();

            ProducerBuilder<TK, TV> builder = new ProducerBuilder<TK, TV>(
                config.Value
            ).SetValueSerializer(new KafkaSerializer<TV>());

            return builder.Build();
        });

        services.AddSingleton<KafkaProducer<TK, TV>>();

        services.Configure(configAction);

        services.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TK, TV, THandler>(
        this IServiceCollection services,
        Action<KafkaConsumerConfig<TK, TV>> configAction
    )
        where THandler : class, IKafkaConsumerHandler<TK, TV>
    {
        services.AddScoped<IKafkaConsumerHandler<TK, TV>, THandler>();

        services.AddHostedService<BackGroundKafkaConsumer<TK, TV>>();

        services.Configure(configAction);

        return services;
    }
}
