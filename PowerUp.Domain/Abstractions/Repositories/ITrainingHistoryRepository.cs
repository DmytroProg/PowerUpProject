using PowerUp.Domain.Models.TrainingHistories;
using PowerUp.Domain.Requests.TrainingHistory;

namespace PowerUp.Domain.Abstractions.Repositories;

public interface ITrainingHistoryRepository
{
    Task<TrainingHistory[]> GetAll(TrainingHistoryRequest request, CancellationToken cancellationToken);
    ValueTask<TrainingHistory?> GetById(int id, CancellationToken cancellationToken);
    Task<TrainingHistory?> GetPreviousTraining(int trainingId, CancellationToken cancellationToken);
    
    void Add(TrainingHistory trainingHistory);
}