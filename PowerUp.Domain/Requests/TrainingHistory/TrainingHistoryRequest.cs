namespace PowerUp.Domain.Requests.TrainingHistory;

public class TrainingHistoryRequest
{
    public int UserId { get; set; }
    
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public int Limit { get; set; }
    public int Offset { get; set; }
}