using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Consumer;
using PlcKafkaLibrary.Producer;
using PlcKafkaLibrary.Service;
using PlcKafkaLibrary.Data;

namespace PlcKafkaLibrary;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaMessageBus(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TK, TV>(
        this IServiceCollection services,
        Action<KafkaProducerConfig<TK, TV>> configAction
    )
    {
        services.AddSingleton(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<
                IOptions<KafkaProducerConfig<TK, TV>>
            >();
            var builder = new ProducerBuilder<TK, TV>(config.Value).SetValueSerializer(
                new KafkaSerializer<TV>()
            );
            return builder.Build();
        });

        services.AddSingleton<KafkaProducer<TK, TV>>();

        services.Configure(configAction);

        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TK, TV, THandler>(
        this IServiceCollection services,
        Action<KafkaConsumerConfig<TK, TV>> configAction
    )
        where THandler : class, IKafkaHandler<TK, TV>
    {
        services.AddScoped<IKafkaHandler<TK, TV>, THandler>();

        services.AddHostedService<BackGroundKafkaConsumer<TK, TV>>();

        services.Configure(configAction);

        return services;
    }
}
