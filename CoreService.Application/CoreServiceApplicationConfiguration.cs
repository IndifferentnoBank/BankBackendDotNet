using System.Reflection;
using CoreService.Application.Helpers.BankAccountNumberGenerator;
using CoreService.Application.Helpers.TransactionExecutor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Application;

public static class CoreServiceApplicationConfiguration
{
    public static void ConfigureCoreServiceApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<IBankAccountNumberGenerator, BankAccountNumberGenerator>();
        builder.Services.AddScoped<ITransactionExecutor, TransactionExecutor>();

    }
}