using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PowerUp.Application.Services.Auth;
using PowerUp.Application.Services.Auth.Jwt;
using PowerUp.Application.Services.Exercises;
using PowerUp.Application.Services.Trainings;
using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Infrastructure;
using PowerUp.Infrastructure.Repositories;

namespace PowerUp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPowerUpApi(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
        services.AddDbContext<PowerUpContext>(options => options.UseInMemoryDatabase("TestPowerUpDB"));
        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<PowerUpContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<IExercisesRepository, ExercisesRepository>();
        
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        services.AddScoped<AuthService>();
        services.AddScoped<TrainingsService>();
        services.AddScoped<ExercisesService>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                };
            });
        
        return services;
    }
}