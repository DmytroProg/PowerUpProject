using PowerUp.Domain.Models.Trainings;

namespace PowerUp.Application.Services.Trainings.Histories;

public class ExerciseData
{
    public required ExerciseGeneralData ExerciseGeneralData { get; set; }
    public required ExerciseSetsData ExerciseSetsData { get; set; }
    public decimal MaxWeight { get; set; }
}

public class ExerciseGeneralData
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Rating { get; set; }
    public Muscle? PrimaryMuscle { get; set; }
    public ICollection<Muscle> SecondaryMuscles { get; set; } = new List<Muscle>();
}

public class ExerciseSetsData
{
    public int Number { get; set; }
    public decimal Weight { get; set; }
    public decimal Count { get; set; }
}