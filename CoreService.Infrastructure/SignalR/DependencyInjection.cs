using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Infrastructure.SignalR;

public static class DependencyInjection
{
    public static void ConfigureCoreServiceSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<TransactionHub>();
    }

    public static void ConfigureCoreServiceSignalR(this WebApplication app)
    {
        app.MapHub<TransactionHub>("/transactionHub");
    }
}