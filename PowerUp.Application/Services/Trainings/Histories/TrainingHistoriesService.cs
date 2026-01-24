using System.Text.Json;
using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.Models.TrainingHistories;
using PowerUp.Domain.Requests.TrainingHistory;
using PowerUp.Domain.Responses;

namespace PowerUp.Application.Services.Trainings.Histories;

public class TrainingHistoriesService
{
    private readonly ITrainingHistoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TrainingHistoriesService(ITrainingHistoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseList<TrainingHistoryResponse>> GetAll(TrainingHistoryRequest request, CancellationToken cancellationToken = default)
    {
        var trainings = await _repository.GetAll(request, cancellationToken);
        
        return trainings.ToResponseList(ToTrainingHistoryResponse);
    }
    
    public async Task<TrainingHistoryResponse?> GetById(int trainingHistoryId, CancellationToken cancellationToken = default)
    {
        var training = await _repository.GetById(trainingHistoryId, cancellationToken);
        
        return training == null ? null : ToTrainingHistoryResponse(training);
    }

    public async Task<TrainingHistoryResponse?> GetPreviousTraining(int userId, int trainingId, CancellationToken cancellationToken = default)
    {
        var training = await _repository.GetPreviousTraining(userId, trainingId, cancellationToken);
        
        return training == null ? null : ToTrainingHistoryResponse(training);
    }

    public async Task<TrainingStatisticsData[]> GetTrainingStatistics(int userId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default)
    {
        if(endDate < startDate)
            throw new ArgumentException("End date must be greater than start date");

        var filter = new TrainingHistoryRequest
        {
            StartTime = startDate,
            EndTime = endDate,
            UserId = userId,
            Offset = 0,
            Limit = 1000
        };
        
        var stats = await _repository.GetAll(filter, cancellationToken);

        return stats.Items.Select(x =>
            {
                var training = JsonSerializer.Deserialize<TrainingData>(x.TrainingState);

                return training == null ? null : new TrainingStatisticsData(x.TrainingStartTime, training.Name);
            })
            .Where(x => x != null)
            .ToArray()!;
    }
    

    public async Task AddTrainingHistory(SaveTrainingHistoryRequest request, CancellationToken cancellationToken = default)
    {
        var trainingHistory = new TrainingHistory
        {
            UserId = request.UserId,
            TrainingId = request.TrainingData.Id,
            TrainingState = JsonSerializer.Serialize(request.TrainingData),
            TrainingStartTime = request.TrainingStartTime,
            ExerciseHistories = request.ExerciseHistories.Select(x => new ExerciseHistory
            {
                ExerciseState = JsonSerializer.Serialize(x.ExerciseGeneralData),
                SetsHistory = JsonSerializer.Serialize(x.ExerciseSetsData),
                MaxWeight = x.MaxWeight
            }).ToList(),
        };
        
        _repository.Add(trainingHistory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    private TrainingHistoryResponse ToTrainingHistoryResponse(TrainingHistory training)
    {
        return new TrainingHistoryResponse
        {
            Id = training.Id,
            TrainingId = training.TrainingId,
            TrainingStartTime = training.TrainingStartTime,
            TrainingResponse = JsonSerializer.Deserialize<TrainingData>(training.TrainingState)!,
            UserId = training.UserId,
            ExerciseHistories = training.ExerciseHistories
                .Select(x => new ExerciseData
                {
                    MaxWeight = x.MaxWeight,
                    ExerciseGeneralData = JsonSerializer.Deserialize<ExerciseGeneralData>(x.ExerciseState)!,
                    ExerciseSetsData = JsonSerializer.Deserialize<ExerciseSetsData>(x.SetsHistory)!
                })
                .ToList()
        };
    }
}