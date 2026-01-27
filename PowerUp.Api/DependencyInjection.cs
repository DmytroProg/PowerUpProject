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

        services.Scan(scan => scan
            .FromAssembliesOf(
                typeof(PowerUpContext),
                typeof(AuthService)
            )
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            .AddClasses(c => c.Where(t =>
                t.Name.EndsWith("Service", StringComparison.Ordinal) &&
                t != typeof(NotificationService)))
            .AsSelf()
            .WithScopedLifetime());

        services.AddTransient<IJwtGenerator, JwtGenerator>();
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
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
        
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chatHub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        
        return services;
    }
}