namespace PowerUp.Application.Services.Trainings.Histories;

public class TrainingHistoryResponse
{
    public int Id { get; set; }

    public int TrainingId { get; set; }

    public int UserId { get; set; }

    public required TrainingData TrainingResponse { get; set; }

    public ICollection<ExerciseData> ExerciseHistories { get; set; } = new List<ExerciseData>();

    public DateTime TrainingStartTime { get; set; }
}