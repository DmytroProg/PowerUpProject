using PowerUp.Application.Services.Auth.Jwt;
using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.Enums;
using PowerUp.Domain.Models.Users;
using PowerUp.Shared.Hash;

namespace PowerUp.Application.Services.Auth;

public class AuthService
{
    private readonly IJwtGenerator _generator;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IJwtGenerator generator, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _generator = generator;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Login(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(request.Email, cancellationToken);

        if (user == null)
        {
           throw new ArgumentException($"User with email {request.Email} not found");
        }

        if (!HasherUtil.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new ArgumentException($"User with email {request.Email} does not match password");
        }
        
        return new AuthResponse
        {
            Token = _generator.GenerateToken(request.Email),
            UserId = user.Id
        };
    }

    public async Task<AuthResponse> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        // validation

        var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);

        if (existingUser != null)
        {
            throw new ArgumentException($"User with email {request.Email} already exists");
        }
        
        var user = new User
        {
            Email = request.Email,
            PasswordHash = HasherUtil.HashPassword(request.Password),
            DateOfBirth = request.DateOfBirth,
            NickName = request.NickName,
            CreatedAt = DateTime.UtcNow,
            Role = UserRole.User
        };
        
        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AuthResponse
        {
            Token = _generator.GenerateToken(request.Email),
            UserId = user.Id
        };
    }
}