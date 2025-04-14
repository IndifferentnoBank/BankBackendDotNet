using Common.Configurations;
using CoreService.Contracts.Interfaces;
using CoreService.Infrastructure.ExternalServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreService.Infrastructure.ExternalServices;

public static class DependencyInjection
{
    public static void ConfigureExternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<HttpClientsConfig>(
            builder.Configuration.GetSection("HttpClients"));
        
        builder.Services.AddHttpClient("UserServiceClient", (sp,client) =>
        {
            var config = sp.GetRequiredService<IOptions<HttpClientsConfig>>().Value.UserServiceClient;
            
            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient("CurrencyServiceClient", (sp,client) =>
        {
            var config = sp.GetRequiredService<IOptions<HttpClientsConfig>>().Value.CurrencyServiceClient;
            
            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
    }
}