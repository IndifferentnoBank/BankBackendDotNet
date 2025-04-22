using CreditRatingService.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Builder;

namespace CreditRatingService.Infrastucture;

public static class DependencyInjection
{
    public static void ConfigureCreditRatingServiceInfrastructure(this WebApplicationBuilder builder)
    {
        builder.ConfigureExternalServices();
    }
}