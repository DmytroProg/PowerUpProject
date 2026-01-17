using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Exercises;
using PowerUp.Domain.Responses;
using PowerUp.Infrastructure.Repositories.Base;

namespace PowerUp.Infrastructure.Repositories;

public class ExercisesRepository : RepositoryBase<Exercise>, IExercisesRepository
{
    public ExercisesRepository(PowerUpContext context) : base(context)
    {
    }

    public async Task<ResponseList<Exercise>> GetExercises(ExercisesRequest request, CancellationToken cancellationToken)
    {
        var query = Set<Exercise>();
        
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(e =>
                e.Name.Contains(request.Search) ||
                e.Description.Contains(request.Search));
        }
        
        if (request.MinRating.HasValue)
        {
            query = query.Where(e => e.Rating >= request.MinRating.Value);
        }

        if (request.MaxRating.HasValue)
        {
            query = query.Where(e => e.Rating <= request.MaxRating.Value);
        }
        

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return new ResponseList<Exercise>
        {
            Items = items,
            TotalCount = count,
            Limit = request.Limit,
            Offset = request.Offset,
            Page = request.Offset / request.Limit
        };
    }

    public Task<Exercise[]> GetExercisesByIds(int[] ids, CancellationToken cancellationToken)
    {
        return Set<Exercise>()
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<ResponseList<Exercise>> GetAllExercisesByTrainingId(int trainingId, CancellationToken cancellationToken)
    {
        var training = await Set<Training>()
            .AsNoTracking()
            .Include(t => t.Exercises)
            .SingleOrDefaultAsync(t => t.Id == trainingId, cancellationToken);
        
        var exercises = training == null ? Array.Empty<Exercise>() : training.Exercises;
        
        return new ResponseList<Exercise>
        {
            Items = exercises,
            TotalCount = exercises.Count,
            Limit = exercises.Count,
            Offset = 0,
            Page = 0
        };
    }
}