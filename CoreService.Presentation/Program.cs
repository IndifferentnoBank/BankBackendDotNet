using Common.Configurations;
using CoreService.Application;
using CoreService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.ConfigureCoreServicePersistence();
builder.ConfigureCoreServiceApplication();
builder.ConfigureSwagger();

var app = builder.Build();

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