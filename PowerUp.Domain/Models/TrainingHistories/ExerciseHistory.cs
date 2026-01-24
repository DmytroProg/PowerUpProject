using System.ComponentModel.DataAnnotations.Schema;

namespace PowerUp.Domain.Models.TrainingHistories;

public class ExerciseHistory
{
    public int Id { get; set; }

    [ForeignKey(nameof(TrainingHistory))]
    public int TrainingHistoryId { get; set; }
    public required TrainingHistory TrainingHistory { get; set; }

    public required string ExerciseState { get; set; }

    public required string SetsHistory { get; set; }

    public decimal MaxWeight { get; set; }
}