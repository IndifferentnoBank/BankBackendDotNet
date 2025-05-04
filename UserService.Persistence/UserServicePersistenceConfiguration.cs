using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Persistence.Repositories.ExpiredTokensRepository;
using UserService.Persistence.Repositories.UserRepository;

namespace UserService.Persistence
{
    public static class UserServicePersistenceConfiguration
    {
        public static void ConfigureUserServicePersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<UserServiceDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb")));

            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IExpiredTokensRepository, ExpiredTokensRepository>();
        }

        public static void ConfigureUserServicePersistence(this WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
            context.Database.Migrate();
        }
    }
}