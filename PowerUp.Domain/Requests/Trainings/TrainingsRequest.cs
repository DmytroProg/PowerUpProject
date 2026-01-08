namespace PowerUp.Domain.Requests.Trainings;

public class TrainingsRequest
{
    public string? SearchField { get; set; }

    public int Limit { get; set; }
    public int Offset { get; set; }
}