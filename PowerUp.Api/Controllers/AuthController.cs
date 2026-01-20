using Microsoft.AspNetCore.Mvc;
using PowerUp.Application.Services.Auth;

namespace PowerUp.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.Login(request, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.Register(request, cancellationToken);
        
        return Ok(response);
    }
}