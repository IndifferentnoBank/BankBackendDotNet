using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using UserService.Application.Services;

namespace UserService.Application
{
    public static class UserServiceApplicationConfig
    {
        public static void ConfigureUserServiceApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserService, Services.UserService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
        }
    }

}
