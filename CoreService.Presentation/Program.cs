using Common.Configurations;
using CoreService.Application;
using CoreService.Application.BackgroundService;
using CoreService.Infrastructure;
using CoreService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.ConfigureCoreServicePersistence();
builder.ConfigureCoreServiceApplication();
builder.ConfigureCoreServiceInfrastructure();
builder.ConfigureSwagger();

var app = builder.Build();

await QuartzScheduler.StartTransactionScheduler(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.MapControllers();

app.UseMiddleware();

app.Run();