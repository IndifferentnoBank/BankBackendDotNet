using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
