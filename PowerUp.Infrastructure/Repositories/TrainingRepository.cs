using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstractions.Repositories;
using PowerUp.Domain.Models.Trainings;
using PowerUp.Domain.Requests.Trainings;
using PowerUp.Domain.Responses;
using PowerUp.Infrastructure.Repositories.Base;

namespace PowerUp.Infrastructure.Repositories;

public sealed class TrainingRepository : RepositoryBase<Training>, ITrainingRepository
{
    public TrainingRepository(PowerUpContext context) : base(context)
    {
    }
    
    public async Task<ResponseList<Training>> GetAll(TrainingsRequest request, CancellationToken cancellationToken)
    {
        var query = Set<Training>();

        if (!string.IsNullOrEmpty(request.SearchField))
        {
            query = query.Where(x => x.Name.Contains(request.SearchField));
        }
        
        var count = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return new ResponseList<Training>
        {
            Items = items,
            TotalCount = count,
            Limit = request.Limit,
            Offset = request.Offset,
            Page = request.Offset / request.Limit,
        };
    }
}