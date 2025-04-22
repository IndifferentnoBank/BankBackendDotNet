using Common.Configurations;
using Microsoft.AspNetCore.Authorization;
using CreditRatingService.Presentation.Authorization;

namespace CreditRatingService.Presentation.Authorization;

public static class DependencyInjection
{
    public static void ConfigureUserServiceAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.AddAuth();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CustomPolicy", policy =>
                policy.Requirements.Add(new AuthorizationRequirements()));
        });

        builder.Services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
    }
}