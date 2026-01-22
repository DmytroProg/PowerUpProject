namespace PowerUp.Application.Services.Users;

public class UserResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string NickName { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public bool IsVerified { get; set; }
}