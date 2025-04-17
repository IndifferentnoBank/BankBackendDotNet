using Common.Kafka.Configs;
using Common.Kafka.Consumer;
using Common.Kafka.Producer;
using Confluent.Kafka;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Kafka.Events;
using CoreService.Kafka.Config;
using CoreService.Kafka.Consumers;
using CoreService.Kafka.Consumers.ExpiredTokens;
using CoreService.Kafka.Consumers.Transactions;
using CoreService.Kafka.KafkaTopicsInitializer;
using CoreService.Kafka.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Kafka;

public static class DependencyInjection
{
    public static void AddKafka(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<BankTransactionsConsumerConfig>(
            builder.Configuration.GetSection("Kafka:Consumers:BankTransactionsConsumer"));

        builder.Services.Configure<ExpiredTokensConsumerConfig>(
            builder.Configuration.GetSection("Kafka:Consumers:ExpiredTokensConsumer"));

        builder.Services.Configure<KafkaProducerConfiguration>(
            builder.Configuration.GetSection("Kafka:Producers:TransactionProducer"));

        builder.Services.AddSingleton<IKafkaProducer<TransactionEvent>, KafkaProducer<TransactionEvent>>();
        builder.Services.AddSingleton<ITransactionProducer, TransactionProducer>();

        builder.Services.AddScoped<TransactionConsumer>();
        builder.Services.AddScoped<ExpiredTokensConsumer>();

        builder.Services.AddHostedService<TransactionConsumerService>();
        builder.Services.AddHostedService<ExpiredTokensConsumerService>();

        builder.Services.AddTransient<IKafkaTopicsInitializer, KafkaTopicsInitialization>();
    }
    
    public static async Task AddKafka(this WebApplication application)
    {
        var scope =  application.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IKafkaTopicsInitializer>();
        await initializer.InitializeTopicsAsync();
    }
}