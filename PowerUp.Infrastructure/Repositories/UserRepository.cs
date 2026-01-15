using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstarctions.Repositories;
using PowerUp.Domain.Models.Users;
using PowerUp.Infrastructure.Repositories.Base;

namespace PowerUp.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(PowerUpContext context) : base(context)
    {
    }

    public Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return Set<User>().FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
    }

    public Task<bool> IsEmailExist(string email, CancellationToken cancellationToken)
    {
        return Set<User>().AnyAsync(u => u.Email.Equals(email.Trim()), cancellationToken);
    }
}