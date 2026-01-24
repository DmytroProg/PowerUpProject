using System.ComponentModel.DataAnnotations.Schema;
using PowerUp.Domain.Models.Users;

namespace PowerUp.Domain.Models.TrainingHistory;

public class TrainingHistory
{
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public required User User { get; set; }

    public required string TrainingState { get; set; }

    public DateTime TrainingStartTime { get; set; }
}