using System.Reflection;
using CoreService.Application.BackgroundService;
using CoreService.Application.Helpers;
using CoreService.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CoreService.Application;

public static class DependencyInjection
{
    public static void ConfigureCoreServiceApplication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CommissionSettings>(builder.Configuration.GetSection("Commissions"));

        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<IBankAccountNumberGenerator, BankAccountNumberGenerator>();
        builder.Services.AddScoped<ITransactionExecutor, TransactionExecutor>();
        builder.Services.AddScoped<ICommissionService, CommissionService>();
        
        builder.Services.AddQuartz();
        builder.Services.AddQuartzHostedService();
        
        builder.Services.AddScoped<TransactionRetryJob>();
    }
}