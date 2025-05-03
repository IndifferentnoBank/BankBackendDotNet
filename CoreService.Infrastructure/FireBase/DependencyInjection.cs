using CoreService.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Infrastructure.FireBase;

public static class DependencyInjection
{
    public static void AddFirebase(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IFireBaseSender, FireBaseSender>();
    }
}