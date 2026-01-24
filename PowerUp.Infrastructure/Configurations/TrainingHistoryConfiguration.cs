using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerUp.Domain.Models.TrainingHistories;

namespace PowerUp.Infrastructure.Configurations;

public class TrainingHistoryConfiguration : IEntityTypeConfiguration<TrainingHistory>
{
    public void Configure(EntityTypeBuilder<TrainingHistory> builder)
    {
        builder.HasKey(th => th.Id);
    }
}

public class ExerciseHistoryConfiguration : IEntityTypeConfiguration<ExerciseHistory>
{
    public void Configure(EntityTypeBuilder<ExerciseHistory> builder)
    {
        builder.HasKey(eh => eh.Id);
    }
}