using CoreService.Infrastructure.ExternalServices;
using CoreService.Infrastructure.FireBase;
using CoreService.Infrastructure.SignalR;
using Microsoft.AspNetCore.Builder;

namespace CoreService.Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureCoreServiceInfrastructure(this WebApplicationBuilder builder)
    {
        builder.ConfigureExternalServices();
        builder.ConfigureCoreServiceSignalR();
        builder.AddFirebase();
    }
    
    public static void ConfigureCoreServiceInfrastructure(this WebApplication app)
    {
        app.ConfigureCoreServiceSignalR();
    }
}