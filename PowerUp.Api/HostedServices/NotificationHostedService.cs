using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Models.Users;
using PowerUp.Infrastructure;

namespace PowerUp.Api.HostedServices;

public class NotificationHostedService : IHostedService
{
    private Timer _timer;
   
    private readonly IServiceProvider _serviceProvider;

    public NotificationHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async _ => await Callback(), null, TimeSpan.FromSeconds(10), TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _timer.DisposeAsync();
    }

    private async Task Callback()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var userRepo = scope.ServiceProvider.GetRequiredService<PowerUpContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
        var usersCount = await userRepo.Set<User>().CountAsync();

        var skip = 0;
        while (usersCount > 0)
        {
            /*
             * SELECT [u].[Id], [u].[CreatedAt], [u].[DateOfBirth], [u].[DeletedAt], [u].[Email], [u].[IsVerified], [u].[NickName], [u].[PasswordHash], [u].[Role]
      FROM [User] AS [u]
      WHERE [u].[IsVerified] = CAST(0 AS bit)
      ORDER BY [u].[Id]
      OFFSET @p ROWS FETCH NEXT @p0 ROWS ONLY
             * 
             */
            
            var emails = await userRepo.Set<User>()
                .AsNoTracking()
                .Where(x => !x.IsVerified)
                .OrderBy(x => x.Id)
                .Select(x => x.Email)
                .Skip(skip)
                .Take(100)
                .ToArrayAsync();

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 20
            };
        
            await Parallel.ForEachAsync(emails, parallelOptions, async (email, ct) =>
            {
                await notificationService.Notify(email, "Notification", "You are not verified yet!!!!!");
            });
            usersCount -= 100;
            skip += 100;
        }
    }
}