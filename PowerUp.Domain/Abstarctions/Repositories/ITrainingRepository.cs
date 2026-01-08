using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Trainings;

namespace PowerUp.Domain.Abstarctions.Repositories;

public interface ITrainingRepository
{
    Task<List<Training>> GetAll(TrainingsRequest request, CancellationToken cancellationToken);
    ValueTask<Training?> GetById(int id, CancellationToken cancellationToken);
    
    void Add(Training training);
    void Update(Training training);
    void Delete(Training training);
}