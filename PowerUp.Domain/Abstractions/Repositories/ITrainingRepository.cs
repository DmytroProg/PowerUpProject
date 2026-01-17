using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Trainings;
using PowerUp.Domain.Responses;

namespace PowerUp.Domain.Abstractions.Repositories;

public interface ITrainingRepository
{
    Task<ResponseList<Training>> GetAll(TrainingsRequest request, CancellationToken cancellationToken);
    ValueTask<Training?> GetById(int id, CancellationToken cancellationToken);
    
    void Add(Training training);
    void Update(Training training);
    void Delete(Training training);
}