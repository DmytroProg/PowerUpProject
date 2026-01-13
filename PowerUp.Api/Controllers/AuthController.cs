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
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        return Ok(await _authService.Login(request));
    }
}