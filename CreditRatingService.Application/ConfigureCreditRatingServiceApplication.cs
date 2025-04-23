using CreditRatingService.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CreditRatingService.Application
{
    public static class CreditRatingApplicationConfig
    {
        public static void ConfigureCreditRatingServiceApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICreditRatingService, Services.CreditRatingService>();
            builder.Services.AddScoped<IOverdueTransactionService, OverdueTransactionService>();
        }
    }
}
