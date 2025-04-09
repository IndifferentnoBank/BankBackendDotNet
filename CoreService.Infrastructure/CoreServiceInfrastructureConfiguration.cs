using CoreService.Infrastructure.ExternalServices.UserService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Infrastructure;

public static class CoreServiceInfrastructureConfiguration
{
    public static void ConfigureCoreServiceInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("UserServiceClient", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Microservices:UserInfoServiceUrl"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddScoped<IUserService, UserService>();
    }
}