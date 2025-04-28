using Common.Configurations;
using CoreService.Contracts.Interfaces;
using CoreService.Infrastructure.ExternalServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CoreService.Infrastructure.ExternalServices;

public static class DependencyInjection
{
    public static void ConfigureExternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<HttpClientsConfig>(
            builder.Configuration.GetSection("HttpClients"));

        builder.Services.AddHttpClient("UserServiceClient", (sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<HttpClientsConfig>>().Value.UserServiceClient;

                client.BaseAddress = new Uri(config.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        builder.Services.AddHttpClient("CurrencyServiceClient", (sp, client) =>
        {
            var config = sp.GetRequiredService<IOptions<HttpClientsConfig>>().Value.CurrencyServiceClient;

            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: 0.7,
                samplingDuration: TimeSpan.FromSeconds(30),
                minimumThroughput: 10,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
    }
}