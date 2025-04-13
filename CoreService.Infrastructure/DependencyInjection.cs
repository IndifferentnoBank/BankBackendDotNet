using CoreService.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Builder;

namespace CoreService.Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureCoreServiceInfrastructure(this WebApplicationBuilder builder)
    {
        builder.ConfigureExternalServices();
    }
}