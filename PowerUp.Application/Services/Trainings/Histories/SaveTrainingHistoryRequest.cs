namespace PowerUp.Application.Services.Trainings.Histories;

public class SaveTrainingHistoryRequest
{
    public required TrainingData TrainingData { get; set; }

    public int UserId { get; set; }
    
    public ICollection<ExerciseData> ExerciseHistories { get; set; } = new List<ExerciseData>();

    public DateTime TrainingStartTime { get; set; }
}