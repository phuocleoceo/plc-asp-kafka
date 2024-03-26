using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using PlcKafkaLibrary.Configuration;
using PlcKafkaLibrary.Consumer;
using PlcKafkaLibrary.Producer;
using PlcKafkaLibrary.Admin;

namespace PlcKafkaLibrary;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddKafkaConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<KafkaConfig>(configuration.GetSection("Kafka"));

        services.ConfigureKafkaAdminClient();

        services.AddSingleton<KafkaCreateTopic>();
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        KafkaCreateTopic kafkaCreateTopic = serviceProvider.GetService<KafkaCreateTopic>();
        kafkaCreateTopic.CreateTopic().Wait();

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services
    )
    {
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
