using CreditRatingService.Contracts.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CreditRatingService.Persistence.Repositories.ExpiredTokensRepository;

namespace CreditRatingService.Persistence
{
    public static class CreditRatingServicePersistenceConfiguration
    {

        public static void ConfigureCreditRatingServicePersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CreditRatingServiceDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("CreditRatingDb")));

            builder.Services.AddTransient<IExpiredTokensRepository, ExpiredTokensRepository>();
        }

        public static void ConfigureCreditRatingServicePersistence(this WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CreditRatingServiceDbContext>();
            context.Database.Migrate();
        }
    }
}
