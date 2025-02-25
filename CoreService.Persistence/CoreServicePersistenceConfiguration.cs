
using Common.GenericRepository;
using CoreService.Persistence.Repositories.BankAccountRepository;
using CoreService.Persistence.Repositories.TransactionsRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Persistence;

public static class CoreServicePersistenceConfiguration
{
    public static void ConfigureCoreServicePersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CoreServiceDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostrgesDb")));
        
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IBankAccountRepository, BankAccountRepository>();
        builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();

    }
}