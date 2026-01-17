using Microsoft.AspNetCore.Mvc;
using PowerUp.Api;
using PowerUp.Api.Endpoints;
using PowerUp.Api.Middlewares;
using PowerUp.Application.Services.Exercises;
using PowerUp.Domain.Requests.Exercises;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddTransient<GlobalExceptionHandling>();

builder.Services
    .AddAuthentication(builder.Configuration)
    .AddPowerUpApi();

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

app.Run();