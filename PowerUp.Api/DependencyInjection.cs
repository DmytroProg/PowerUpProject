using Microsoft.EntityFrameworkCore;
using PowerUp.Application.Services.Auth;
using PowerUp.Application.Services.Auth.Jwt;
using PowerUp.Domain.Abstarctions;
using PowerUp.Domain.Abstarctions.Repositories;
using PowerUp.Infrastructure;
using PowerUp.Infrastructure.Repositories;

namespace PowerUp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPowerUpApi(this IServiceCollection services)
    {
        services.AddDbContext<PowerUpContext>(options => options.UseInMemoryDatabase("TestPowerUpDB"));
        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<PowerUpContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        services.AddScoped<AuthService>();

        return services;
    }
}