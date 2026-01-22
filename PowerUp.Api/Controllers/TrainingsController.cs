using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PowerUp.Application.Services.Trainings;
using PowerUp.Domain.Requests.Trainings;

namespace PowerUp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/trainings")]
public class TrainingsController : ControllerBase
{
    private const string TrainingsCacheKey = "trainings";
    
    private readonly TrainingsService _trainingsService;
    private readonly IMemoryCache _memoryCache;

    public TrainingsController(TrainingsService trainingsService, IMemoryCache memoryCache)
    {
        _trainingsService = trainingsService;
        _memoryCache = memoryCache;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int limit = 10, 
        [FromQuery] int offset = 0, 
        [FromQuery] string? searchField = null, 
        CancellationToken cancellationToken = default)
    {
        var request = new TrainingsRequest
        {
            SearchField = searchField,
            Limit = limit,
            Offset = offset
        };
        var key = $"{TrainingsCacheKey}{JsonSerializer.Serialize(request)}";
        
        if (_memoryCache.TryGetValue(key, out var trainings))
        {
            return Ok(trainings);
        }
        
        var response = await _trainingsService.GetAll(request, cancellationToken);

        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)) // точно стреться через 10 хв
            .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // стреться якщо протягом 1 хв ніхто цей кеш не чіпав
        
        _memoryCache.Set(key, response, options);
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _trainingsService.GetById(id, cancellationToken);
        
        return Ok(response);
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(CreateTrainingRequest request, CancellationToken cancellationToken)
    {
        var training = await _trainingsService.Add(request, cancellationToken);
        
        return Created($"api/v1/trainings/{training.Id}", training);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CreateTrainingRequest request, CancellationToken cancellationToken)
    {
        var training = await _trainingsService.UpdateTraining(id, request, cancellationToken);
        
        return Ok(training);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _trainingsService.DeleteTraining(id, cancellationToken);

        return NoContent();
    }
}