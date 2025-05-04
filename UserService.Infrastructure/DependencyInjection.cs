using Common.Kafka.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.ExpiredTokens;

namespace UserService.Infrastructure;

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