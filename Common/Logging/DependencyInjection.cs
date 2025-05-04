using Common.Configurations;
using Common.Kafka.Configs;
using Common.Kafka.Producer;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Logging;

public static class DependencyInjection
{
    public static void AddKafkaLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceInfoConfig>(
            configuration.GetSection("ServiceInfo"));
        
        var loggingRequestProducer = configuration.GetSection("Kafka:Producers:LoggingRequestProducer")
            .Get<KafkaProducerConfiguration>();

        services.AddTransient<KafkaTracingHandler>();
        
        if (loggingRequestProducer == null) return;
        services.AddSingleton<IProducer<Null, string>>(sp => new ProducerBuilder<Null, string>(new ProducerConfig
            { BootstrapServers = loggingRequestProducer.BootstrapServers }).Build());
    }
}