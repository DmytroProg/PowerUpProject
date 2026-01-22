using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using PowerUp.Api;
using PowerUp.Api.Endpoints;
using PowerUp.Api.HostedServices;
using PowerUp.Api.Middlewares;
using PowerUp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services.AddHostedService<NotificationHostedService>();

builder.Services.AddTransient<GlobalExceptionHandling>();

builder.Services
    .AddAuthentication(builder.Configuration)
    .AddPowerUpApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExercisesEndpoints();
    
app.UseMiddleware<GlobalExceptionHandling>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PowerUpContext>().Database;

    foreach (var migration in db.GetPendingMigrations())
    {
        Console.WriteLine($"Applying {migration}...");
    }
    
    await db.MigrateAsync();
}

app.Run();