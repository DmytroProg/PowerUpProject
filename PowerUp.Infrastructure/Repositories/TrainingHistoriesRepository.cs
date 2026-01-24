using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.Models.TrainingHistories;
using PowerUp.Domain.Requests.TrainingHistory;
using PowerUp.Infrastructure.Repositories.Base;

namespace PowerUp.Infrastructure.Repositories;

public class TrainingHistoriesRepository : RepositoryBase<TrainingHistory>, ITrainingHistoryRepository
{
    public TrainingHistoriesRepository(PowerUpContext context) : base(context)
    {
    }

    public Task<TrainingHistory[]> GetAll(TrainingHistoryRequest request, CancellationToken cancellationToken)
    {
        var trainingHistoriesQuery = Set<TrainingHistory>()
            .Include(th => th.ExerciseHistories)
            .AsNoTracking()
            .Where(th => th.UserId == request.UserId);

        if (request.StartTime.HasValue)
        {
            trainingHistoriesQuery = trainingHistoriesQuery.Where(th => th.TrainingStartTime >= request.StartTime.Value);
        }
        if (request.EndTime.HasValue)
        {
            trainingHistoriesQuery = trainingHistoriesQuery.Where(th => th.TrainingStartTime <= request.EndTime.Value);
        }
        
        return trainingHistoriesQuery
            .OrderByDescending(th => th.TrainingStartTime)
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToArrayAsync(cancellationToken);
    }

    public Task<TrainingHistory?> GetPreviousTraining(int trainingId, CancellationToken cancellationToken)
    {
        return Set<TrainingHistory>()
            .OrderByDescending(th => th.TrainingStartTime)
            .FirstOrDefaultAsync(th => th.TrainingId == trainingId, cancellationToken);
    }
}