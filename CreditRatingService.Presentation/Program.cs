using Common.Configurations.Swagger;
using Common.Middleware;
using CreditRatingService.Presentation.Authorization;
using CreditRatingService.Application;
using CreditRatingService.Persistence;
using Microsoft.AspNetCore.Http;
using CreditRatingService.Infrastucture;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.ConfigureCreditRatingServiceAuthorization();
builder.ConfigureCreditRatingServiceInfrastructure();
builder.ConfigureCreditRatingServicePersistence();
builder.ConfigureCreditRatingServiceApplication();
builder.ConfigureSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerConfiguration();
}

app.ConfigureCreditRatingServicePersistence();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
