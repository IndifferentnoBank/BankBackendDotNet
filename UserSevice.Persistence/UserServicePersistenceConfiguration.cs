using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserSevice.Persistence
{
    public static class UserServicePersistenceConfiguration
    {
        
        public static void ConfigureUserServicePersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<UserServiceDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb")));
            
        }
    }
}
