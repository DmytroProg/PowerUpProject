using Microsoft.Extensions.Logging;
using PowerUp.Domain.Abstractions;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.CustomExceptions;
using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Trainings;
using PowerUp.Domain.Responses;

namespace PowerUp.Application.Services.Trainings;

public class TrainingsService
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TrainingsService> _logger;

    public TrainingsService(ITrainingRepository trainingRepository, IUnitOfWork unitOfWork, ILogger<TrainingsService> logger)
    {
        _trainingRepository = trainingRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResponseList<TrainingResponse>> GetAll(TrainingsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _trainingRepository.GetAll(request, cancellationToken);

        return response.ToResponseList(ToTrainingResponse);
    }
    
    public async Task<TrainingResponse?> GetById(int id, CancellationToken cancellationToken = default)
    {
        var response = await _trainingRepository.GetById(id, cancellationToken);

        return response == null 
            ? null 
            : ToTrainingResponse(response);
    }

    public async Task<TrainingResponse> Add(CreateTrainingRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Invalid training name");
        }

        try
        {
            var training = new Training
            {
                Name = request.Name,
                Rating = 0,
                IntervalTime = request.IntervalTime,
                DifficultyLevel = request.DifficultyLevel,
                TrainingFormat = request.TrainingFormat,
                TrainingGoal = request.TrainingGoal,
                TrainingIntensity = request.TrainingIntensity,
                TrainingStructure = request.TrainingStructure,
                TrainingType = request.TrainingType
            };

            _trainingRepository.Add(training);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Added new training {training.Name}");

            return ToTrainingResponse(training);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<TrainingResponse> UpdateTraining(int id, CreateTrainingRequest request,
        CancellationToken cancellationToken = default)
    {
        var training = await _trainingRepository.GetById(id, cancellationToken);
        
        if (training == null)
            throw new NotFoundException("Training not found");
        
        training.Name = request.Name;
        training.DifficultyLevel = request.DifficultyLevel;
        training.TrainingFormat = request.TrainingFormat;
        training.TrainingGoal = request.TrainingGoal;
        training.TrainingIntensity = request.TrainingIntensity;
        training.TrainingStructure = request.TrainingStructure;
        training.TrainingType = request.TrainingType;
        
        // not important actually
        _trainingRepository.Update(training);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Updated {training.Name}");
        return ToTrainingResponse(training);
    }

    public async Task DeleteTraining(int id, CancellationToken cancellationToken = default)
    {
        var training = await _trainingRepository.GetById(id, cancellationToken);
        
        if (training == null)
            throw new NotFoundException("Training not found");
        
        _trainingRepository.Delete(training);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private TrainingResponse ToTrainingResponse(Training training)
    {
        return new TrainingResponse
        {
            Id = training.Id,
            Name = training.Name,
            Rating = training.Rating,
            IntervalTime = training.IntervalTime,
            DifficultyLevel = training.DifficultyLevel,
            TrainingFormat = training.TrainingFormat,
            TrainingGoal = training.TrainingGoal,
            TrainingIntensity = training.TrainingIntensity,
            TrainingStructure = training.TrainingStructure,
            TrainingType = training.TrainingType
        };
    } 
}