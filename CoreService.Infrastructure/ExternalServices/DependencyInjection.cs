using CoreService.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Infrastructure.ExternalServices;

public static class DependencyInjection
{
    public static void ConfigureExternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("UserServiceClient", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Microservices:UserInfoServiceUrl"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddScoped<IUserService, Services.UserService>();
    }
}