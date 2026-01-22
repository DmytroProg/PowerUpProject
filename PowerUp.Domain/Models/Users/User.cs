using PowerUp.Domain.Enums;
using PowerUp.Domain.Models.Base;

namespace PowerUp.Domain.Models.Users;

public class User : BaseEntity
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string NickName { get; set; }
    public required DateTime DateOfBirth { get; set; }

    public bool IsVerified { get; set; }

    public UserRole Role { get; set; }
}