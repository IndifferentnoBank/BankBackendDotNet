using System.Reflection;
using CoreService.Application.BackgroundService;
using CoreService.Application.Helpers.BankAccountNumberGenerator;
using CoreService.Application.Helpers.TransactionExecutor;
using CoreService.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CoreService.Application;

public static class DependencyInjection
{
    public static void ConfigureCoreServiceApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<IBankAccountNumberGenerator, BankAccountNumberGenerator>();
        builder.Services.AddScoped<ITransactionExecutor, TransactionExecutor>();
        
        builder.Services.AddQuartz();
        builder.Services.AddQuartzHostedService();
        
        builder.Services.AddScoped<TransactionRetryJob>();
    }
}