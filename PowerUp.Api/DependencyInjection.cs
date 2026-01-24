using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PowerUp.Application.Services.Auth;
using PowerUp.Application.Services.Auth.Jwt;
using PowerUp.Application.Services.Exercises;
using PowerUp.Application.Services.Trainings;
using PowerUp.Application.Services.Trainings.Histories;
using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Infrastructure;
using PowerUp.Infrastructure.Notifications;
using PowerUp.Infrastructure.Repositories;

namespace PowerUp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPowerUpApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        
        services.AddDbContext<PowerUpContext>(options => options.UseSqlServer(configuration.GetConnectionString("Local")));
        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<PowerUpContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<IExercisesRepository, ExercisesRepository>();
        services.AddScoped<ITrainingHistoryRepository, TrainingHistoriesRepository>();
        
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        services.AddScoped<AuthService>();
        services.AddScoped<TrainingsService>();
        services.AddScoped<ExercisesService>();
        services.AddScoped<TrainingHistoriesService>();

        services.AddTransient<INotificationService, NotificationService>();

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