using PowerUp.Domain.Models.Users;

namespace PowerUp.Application.Services.Auth;

public class AuthResponse
{
    public required string Token { get; set; }
    public required int UserId { get; set; }
}