using Common.Configurations.Swagger;
using Common.Middleware;
using UserService.Presentation.Authorization;
using UserService.Application;
using UserSevice.Persistence;
using UserService.Infrastucture;
using UserService.Presentation.Extensions;
using Common.Logging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.ConfigureUserServiceAuthorization();
builder.ConfigureUserServicePersistence();
builder.ConfigureUserServiceApplication();
builder.ConfigureUserServiceInfrastructure();
builder.ConfigureSwagger();

builder.Services.AddKafkaLogging(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var app = builder.Build();
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureUserServicePersistence();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseUnstableMiddleware();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
