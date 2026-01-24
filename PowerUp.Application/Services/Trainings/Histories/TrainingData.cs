using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PowerUp.Domain.Enums;

namespace PowerUp.Application.Services.Trainings.Histories;

public class TrainingData
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Rating { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public DifficultyLevel DifficultyLevel { get; set; }
    [JsonProperty]
    [JsonConverter(typeof(StringEnumConverter))]
    public TrainingType TrainingType { get; set; }
    [JsonProperty]
    [JsonConverter(typeof(StringEnumConverter))]
    public TrainingStructure TrainingStructure { get; set; }
    [JsonProperty]
    [JsonConverter(typeof(StringEnumConverter))]
    public TrainingFormat TrainingFormat { get; set; }
    [JsonProperty]
    [JsonConverter(typeof(StringEnumConverter))]
    public TrainingGoal TrainingGoal { get; set; }
    [JsonProperty]
    [JsonConverter(typeof(StringEnumConverter))]
    public TrainingIntensity TrainingIntensity { get; set; }
    
    public int IntervalTime { get; set; }
}