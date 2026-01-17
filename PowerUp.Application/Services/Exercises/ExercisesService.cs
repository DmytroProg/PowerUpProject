using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.CustomExceptions;
using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Exercises;
using PowerUp.Domain.Responses;

namespace PowerUp.Application.Services.Exercises;

public class ExercisesService
{
    private readonly IExercisesRepository _exercisesRepository;
    private readonly ITrainingRepository _trainingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExercisesService(IExercisesRepository exercisesRepository, IUnitOfWork unitOfWork, ITrainingRepository trainingRepository)
    {
        _exercisesRepository = exercisesRepository;
        _unitOfWork = unitOfWork;
        _trainingRepository = trainingRepository;
    }

    public async Task<ResponseList<ExerciseResponse>> GetExercises(ExercisesRequest request, CancellationToken cancellationToken)
    {
        var exercises = await _exercisesRepository.GetExercises(request, cancellationToken);

        return exercises.ToResponseList(ToExerciseResponse);
    }

    public async Task<ResponseList<ExerciseResponse>> GetAllExercisesByTrainingId(int trainingId, CancellationToken cancellationToken)
    {
        var exercises = await _exercisesRepository.GetAllExercisesByTrainingId(trainingId, cancellationToken);

        return exercises.ToResponseList(ToExerciseResponse);
    }

    public async Task<ExerciseResponse> AddExercise(CreateExerciseRequest request, CancellationToken cancellationToken)
    {
        // validation

        var similarExercises = await _exercisesRepository.GetExercisesByIds(request.SimilarExercisesIds.ToArray(), cancellationToken);
        var exercise = new Exercise
        {
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            PrimaryMuscle = request.PrimaryMuscle,
            SecondaryMuscles = request.SecondaryMuscles,
            Recommendations = request.Recommendations,
            Rating = request.Rating,
            UnrecommendedInjuries = request.UnrecommendedInjuries,
            SimilarExercises = similarExercises
        };
        
        _exercisesRepository.Add(exercise);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return ToExerciseResponse(exercise);
    }
    
    public async Task<ExerciseResponse> AddExerciseToTraining(int exerciseId, int trainingId, CancellationToken cancellationToken)
    {
        var training = await _trainingRepository.GetById(trainingId, cancellationToken);

        if (training == null)
        {
            throw new NotFoundException($"Training with id {trainingId} not found");
        }
        
        var exercise = await _exercisesRepository.GetById(exerciseId, cancellationToken);

        if (exercise == null)
        {
            throw new NotFoundException($"Exercise with id {exerciseId} not found");
        }
        
        training.Exercises.Add(exercise);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return ToExerciseResponse(exercise);
    }

    public async Task<ExerciseResponse?> GetById(int exerciseId, CancellationToken cancellationToken)
    {
        var exercise = await _exercisesRepository.GetById(exerciseId, cancellationToken);

        if (exercise == null)
            return null;
        
        return ToExerciseResponse(exercise);
    }

    private ExerciseResponse ToExerciseResponse(Exercise exercise)
    {
        return new ExerciseResponse
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            ImageUrl = exercise.ImageUrl,
            PrimaryMuscle = exercise.PrimaryMuscle,
            SecondaryMuscles = exercise.SecondaryMuscles,
            Recommendations = exercise.Recommendations,
            Rating = exercise.Rating,
            UnrecommendedInjuries = exercise.UnrecommendedInjuries,
            SimilarExercises = exercise.SimilarExercises.Select(x => new SimilarExercise(x.Id, x.Name)).ToList()
        };
    }
}