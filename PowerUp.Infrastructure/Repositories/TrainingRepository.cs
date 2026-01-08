using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstarctions.Repositories;
using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Trainings;
using PowerUp.Infrastructure.Repositories.Base;

namespace PowerUp.Infrastructure.Repositories;

public sealed class TrainingRepository : RepositoryBase<Training>, ITrainingRepository
{
    public TrainingRepository(PowerUpContext context) : base(context)
    {
    }
    
    public Task<List<Training>> GetAll(TrainingsRequest request, CancellationToken cancellationToken)
    {
        var query = Set<Training>();

        if (!string.IsNullOrEmpty(request.SearchField))
        {
            query = query.Where(x => x.Name.Contains(request.SearchField));
        }
        
        return query
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);
    }
}