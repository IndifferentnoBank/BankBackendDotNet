using Common.Kafka.Configs;
using UserService.Kafka.Consumers.ExpiredTokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastucture;

public static class DependencyInjection
{
    public static void ConfigureUserServiceInfrastructure(this WebApplicationBuilder builder)
    {

        builder.Services.Configure<ExpiredTokensConsumerConfig>(
           builder.Configuration.GetSection("Kafka:Consumers:ExpiredTokensConsumer"));
        builder.Services.AddScoped<ExpiredTokensConsumer>();
        builder.Services.AddHostedService<ExpiredTokensConsumerService>();
    }
}