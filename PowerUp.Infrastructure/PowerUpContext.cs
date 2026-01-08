using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstarctions;
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
}