using Common.Configurations;
using CreditRatingService.Contracts.Interfaces;
using CreditRatingService.Infrastucture.ExternalServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CreditRatingService.Infrastucture.ExternalServices;

public static class DependencyInjection
{
    public static void ConfigureExternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<HttpClientConfig>(
            builder.Configuration.GetSection("CoreServiceClient"));
        
        builder.Services.AddHttpClient("CoreServiceClient", (sp,client) =>
        {
            var config = sp.GetRequiredService<IOptions<HttpClientConfig>>().Value;
            
            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
       
        builder.Services.AddScoped<ICoreService, CoreService>();

        builder.Services.Configure<HttpClientConfig>(
            builder.Configuration.GetSection("LoanServiceClient"));

        builder.Services.AddHttpClient("LoanServiceClient", (sp, client) =>
        {
            var config = sp.GetRequiredService<IOptions<HttpClientConfig>>().Value;

            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddScoped<ILoanService, LoanService>();
    }
}