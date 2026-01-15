namespace PowerUp.Application.Services.Auth;

public class RegisterUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string NickName { get; set; }
    public required DateTime DateOfBirth { get; set; }
}