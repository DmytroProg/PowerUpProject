namespace PowerUp.Domain.Requests.Exercises;

public class ExercisesRequest
{
    public string? Search { get; set; }
    public int? MinRating { get; set; }
    public int? MaxRating { get; set; }
    public int Limit { get; set; } = 10;
    public int Offset { get; set; }
}