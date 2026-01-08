using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerUp.Domain.Models.Trainings;

namespace PowerUp.Infrastructure.Configurations;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
    public void Configure(EntityTypeBuilder<Training> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasMany(t => t.Exercises)
            .WithMany();

        builder.HasQueryFilter(t => !t.DeletedAt.HasValue);
    }
}