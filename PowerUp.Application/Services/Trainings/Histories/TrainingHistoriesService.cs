using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;

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
    
    // TODO:
    // отримати історію тренувань по фільтру
    // отримати тренування по ід
    // отримати останнє тренування
    // отримати статистику по тренуванях за місяць
    // додати історію тренування
}