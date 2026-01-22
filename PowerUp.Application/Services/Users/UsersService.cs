using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.Models.Users;

namespace PowerUp.Application.Services.Users;

public class UsersService
{
    // TODO:
    // отримати юзера по ід
    // отримати юзерів по пошуку
    // update, delete

    private readonly IUserRepository _repository;

    public UsersService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserResponse?> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById(id, cancellationToken);

        if (user == null)
            return null;
        
        return ToUserResponse(user);
    }

    private UserResponse ToUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            NickName = user.NickName,
            DateOfBirth = user.DateOfBirth,
            IsVerified = user.IsVerified,
        };
    }
}