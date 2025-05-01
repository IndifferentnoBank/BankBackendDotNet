using Common.Kafka.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Logging;

public static class DependencyInjection
{
    public static void AddKafkaLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaProducerConfig = configuration.GetSection("Kafka:Producers:LoggingProducer")
            .Get<KafkaProducerConfiguration>();

        services.AddLogging(loggingBuilder =>
        {
            if (kafkaProducerConfig != null)
                loggingBuilder.AddProvider(new KafkaLoggerProvider(kafkaProducerConfig.BootstrapServers,
                    kafkaProducerConfig.Topic));
        });
    }
}