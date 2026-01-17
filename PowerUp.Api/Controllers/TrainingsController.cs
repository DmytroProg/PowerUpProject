using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerUp.Application.Services.Trainings;
using PowerUp.Domain.Requests.Trainings;

namespace PowerUp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/trainings")]
public class TrainingsController : ControllerBase
{
    private readonly TrainingsService _trainingsService;

    public TrainingsController(TrainingsService trainingsService)
    {
        _trainingsService = trainingsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var request = new TrainingsRequest
        {
            Offset = 0,
            Limit = 100,
            SearchField = null
        };
        
        return Ok();
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
        return Ok("Admin access");
    }
}