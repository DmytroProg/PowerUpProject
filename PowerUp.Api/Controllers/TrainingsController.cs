using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
    public async Task<IActionResult> GetAll([FromBody] TrainingsRequest request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(TrainingsCacheKey, out var trainings))
        {
            return Ok(trainings);
        }
        
        var response = await _trainingsService.GetAll(request, cancellationToken);

        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)) // точно стреться через 10 хв
            .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // стреться якщо протягом 1 хв ніхто цей кеш не чіпав
        
        _memoryCache.Set(TrainingsCacheKey, response, options);
        
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpGet("get2")]
    public async Task<IActionResult> GetAll2()
    {
        var request = new TrainingsRequest
        {
            Offset = 0,
            Limit = 100,
            SearchField = null
        };
        
        return Ok();
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        _memoryCache.Remove(TrainingsCacheKey);
        return Ok("Admin access");
    }
}