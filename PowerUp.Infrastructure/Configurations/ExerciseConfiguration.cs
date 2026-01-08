using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerUp.Domain.Models.Trainings;

namespace PowerUp.Infrastructure.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(x => x.SimilarExercises)
            .WithMany();
        
        builder.HasQueryFilter(t => !t.DeletedAt.HasValue);
    }
}