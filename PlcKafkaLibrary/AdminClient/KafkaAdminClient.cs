using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

using PlcKafkaLibrary.Configuration;

namespace PlcKafkaLibrary.AdminClient;

public static class KafkaAdminClient
{
    public static IServiceCollection ConfigureKafkaAdminClient(this IServiceCollection services)
    {
        services.AddSingleton(serviceProvider =>
        {
            IOptions<KafkaConfig> kafkaConfig = serviceProvider.GetRequiredService<
                IOptions<KafkaConfig>
            >();

            KafkaAdminClientConfig kafkaAdminClientConfig = kafkaConfig.Value.AdminClientConfig;

            return new AdminClientBuilder(kafkaAdminClientConfig).Build();
        });

        return services;
    }
}
