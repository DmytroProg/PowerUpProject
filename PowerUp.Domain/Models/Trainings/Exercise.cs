using System.ComponentModel.DataAnnotations.Schema;
using PowerUp.Domain.Models.Base;

namespace PowerUp.Domain.Models.Trainings;

public class Exercise : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Rating { get; set; }
    [NotMapped]
    public Muscle? PrimaryMuscle { get; set; }
    [NotMapped]
    public ICollection<Muscle> SecondaryMuscles { get; set; } = new List<Muscle>();
    [NotMapped]
    public ICollection<Injury> UnrecommendedInjuries { get; set; } = new List<Injury>();
    public string? Recommendations { get; set; }
    public ICollection<Exercise> SimilarExercises { get; set; } = new List<Exercise>();
}