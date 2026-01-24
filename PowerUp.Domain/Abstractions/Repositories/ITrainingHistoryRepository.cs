using PowerUp.Domain.Models.TrainingHistories;
using PowerUp.Domain.Requests.TrainingHistory;
using PowerUp.Domain.Responses;

namespace PowerUp.Domain.Abstractions.Repositories;

public interface ITrainingHistoryRepository
{
    Task<ResponseList<TrainingHistory>> GetAll(TrainingHistoryRequest request, CancellationToken cancellationToken);
    ValueTask<TrainingHistory?> GetById(int id, CancellationToken cancellationToken);
    Task<TrainingHistory?> GetPreviousTraining(int userId, int trainingId, CancellationToken cancellationToken);
    
    void Add(TrainingHistory trainingHistory);
}