using PowerUp.Domain.Models.Users;

namespace PowerUp.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    ValueTask<User?> GetById(int id, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<bool> IsEmailExist(string email, CancellationToken cancellationToken);
    void Add(User user);
}