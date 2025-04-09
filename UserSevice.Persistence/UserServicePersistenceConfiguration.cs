using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserSevice.Persistence.Repositories.UserRepository;

namespace UserSevice.Persistence
{
    public static class UserServicePersistenceConfiguration
    {
        
        public static void ConfigureUserServicePersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<UserServiceDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb")));

            builder.Services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
