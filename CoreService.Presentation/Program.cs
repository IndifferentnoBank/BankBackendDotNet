using Common.Configurations;
using Common.Configurations.Swagger;
using CoreService.Application;
using CoreService.Application.BackgroundService;
using CoreService.Application.Helpers;
using CoreService.Infrastructure;
using CoreService.Kafka;
using CoreService.Persistence;
using CoreService.Presentation.Authorization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.ConfigureCoreServicePersistence();
builder.ConfigureCoreServiceApplication();
builder.ConfigureCoreServiceInfrastructure();
builder.ConfigureSwagger();
builder.AddKafka();
builder.ConfigureCoreServiceAuthorization();
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
});
var app = builder.Build();

await QuartzScheduler.StartTransactionScheduler(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseCors("AllowAll");
app.ConfigureCoreServiceInfrastructure();
await app.ConfigureCoreServicePersistence();
await app.AddKafka();

var options = app.Services.GetRequiredService<IOptions<CommissionSettings>>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"Commission Rate: {options.Value.Commission}");

app.UseHttpsRedirection();

app.UseMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();