using Common.Configurations;
using CoreService.Application;
using CoreService.Application.BackgroundService;
using CoreService.Infrastructure;
using CoreService.Kafka;
using CoreService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.ConfigureCoreServicePersistence();
builder.ConfigureCoreServiceApplication();
builder.ConfigureCoreServiceInfrastructure();
builder.ConfigureSwagger();
builder.AddKafka();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.ConfigureCoreServiceInfrastructure();
await app.ConfigureCoreServicePersistence();

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware();

app.Run();