using Common.Configurations.Swagger;
using Common.Middleware;
using UserService.Presentation.Authorization;
using UserService.Application;
using UserSevice.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.ConfigureUserServiceAuthorization();
builder.ConfigureUserServicePersistence();
builder.ConfigureUserServiceApplication();
builder.ConfigureSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureUserServicePersistence();

app.UseMiddleware<Middleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
