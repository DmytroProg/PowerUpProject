using System.ComponentModel.DataAnnotations.Schema;
using PowerUp.Domain.Models.Users;

namespace PowerUp.Domain.Models.TrainingHistories;

public class TrainingHistory
{
    public int Id { get; set; }

    public int TrainingId { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public required User User { get; set; }

    public required string TrainingState { get; set; }

    public ICollection<ExerciseHistory> ExerciseHistories { get; set; } = new List<ExerciseHistory>();

    public DateTime TrainingStartTime { get; set; }
}