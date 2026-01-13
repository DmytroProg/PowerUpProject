using PowerUp.Application.Services.Auth.Jwt;

namespace PowerUp.Application.Services.Auth;

public class AuthService
{
    private readonly JwtGenerator _generator;

    public AuthService(JwtGenerator generator)
    {
        _generator = generator;
    }

    public async Task<AuthResponse> Login(LoginUserRequest request)
    {
        // get user from DB
        // repository
        
        return new AuthResponse
        {
            Token = _generator.GenerateToken(request.Email)
        };
    }
}