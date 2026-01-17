using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstractions;
using PowerUp.Infrastructure.Configurations;

namespace PowerUp.Infrastructure;

public class PowerUpContext : DbContext, IUnitOfWork
{
    public PowerUpContext(DbContextOptions<PowerUpContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrainingConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Saved");
        return base.SaveChangesAsync(cancellationToken);
    }
}