using Common.Kafka.Configs;
using CreditRatingService.Infrastucture.ExternalServices;
using CreditRatingService.Kafka.Consumers.ExpiredTokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CreditRatingService.Infrastucture;

public static class DependencyInjection
{
    public static void ConfigureCreditRatingServiceInfrastructure(this WebApplicationBuilder builder)
    {
        builder.ConfigureExternalServices();

        builder.Services.Configure<ExpiredTokensConsumerConfig>(
           builder.Configuration.GetSection("Kafka:Consumers:ExpiredTokensConsumer"));
        builder.Services.AddScoped<ExpiredTokensConsumer>();
        builder.Services.AddHostedService<ExpiredTokensConsumerService>();
    }
}