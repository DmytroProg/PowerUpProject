using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Exercises;
using PowerUp.Domain.Responses;

namespace PowerUp.Domain.Abstractions.Repositories;

public interface IExercisesRepository
{
    Task<ResponseList<Exercise>> GetExercises(ExercisesRequest request, CancellationToken cancellationToken);
    Task<Exercise[]> GetExercisesByIds(int[] ids, CancellationToken cancellationToken);
    ValueTask<Exercise?> GetById(int id, CancellationToken cancellationToken);
    Task<ResponseList<Exercise>> GetAllExercisesByTrainingId(int trainingId, CancellationToken cancellationToken);
    
    void Add(Exercise exercise);
    void Update(Exercise exercise);
    void Delete(Exercise exercise);
}