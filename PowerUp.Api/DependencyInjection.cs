using PowerUp.Application.Services.Auth;
using PowerUp.Application.Services.Auth.Jwt;

namespace PowerUp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPowerUpApi(this IServiceCollection services)
    {
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        services.AddTransient<AuthService>();

        return services;
    }
}