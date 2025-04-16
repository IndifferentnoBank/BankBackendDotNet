using Common.Configurations;
using Common.Kafka.Consumer;
using Common.Kafka.Producer;
using CoreService.Kafka.Consumers;
using CoreService.Contracts.Events;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Kafka.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Kafka;

public static class DependencyInjection
{
    public static void AddKafka(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<KafkaConfiguration>(
            builder.Configuration.GetSection("Kafka:Consumers:BankTransactionsConsumer"));

        builder.Services.AddSingleton<IKafkaProducer<TransactionEvent>, KafkaProducer<TransactionEvent>>();
        builder.Services.AddScoped<IKafkaConsumer, TransactionConsumer>();
        builder.Services.AddSingleton<ITransactionProducer, TransactionProducer>();

        builder.Services.Configure<KafkaConfiguration>(
            builder.Configuration.GetSection("Kafka:Consumers:ExpiredTokensConsumer"));

        builder.Services.AddScoped<IKafkaConsumer, ExpiredTokensConsumer>();
    }
}