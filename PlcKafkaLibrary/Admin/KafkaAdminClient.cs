using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PlcKafkaLibrary.Configuration;

namespace PlcKafkaLibrary.Admin;

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
