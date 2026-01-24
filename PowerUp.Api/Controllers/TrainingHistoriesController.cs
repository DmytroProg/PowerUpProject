using Microsoft.AspNetCore.Mvc;
using PowerUp.Application.Services.Trainings.Histories;
using PowerUp.Domain.Requests.TrainingHistory;

namespace PowerUp.Api.Controllers;

[ApiController]
[Route("api/v1/training-histories")]
public class TrainingHistoriesController : ControllerBase
{
    private readonly TrainingHistoriesService _service;

    public TrainingHistoriesController(TrainingHistoriesService service)
    {
        _service = service;
    }

    // GET api/v1/training-histories?userId=1&startTime=...&endTime=...&limit=50&offset=0
    [HttpGet]
    [ProducesResponseType(typeof(TrainingHistoryResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] TrainingHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetAll(request, cancellationToken);
        return Ok(result);
    }

    // GET api/v1/training-histories/123
    [HttpGet("{trainingHistoryId:int}")]
    [ProducesResponseType(typeof(TrainingHistoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TrainingHistoryResponse>> GetById(
        [FromRoute] int trainingHistoryId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetById(trainingHistoryId, cancellationToken);
        return Ok(result);
    }

    // GET api/v1/training-histories/previous?userId=1&trainingId=10
    [HttpGet("previous")]
    [ProducesResponseType(typeof(TrainingHistoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TrainingHistoryResponse>> GetPreviousTraining(
        [FromQuery] int userId,
        [FromQuery] int trainingId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPreviousTraining(userId, trainingId, cancellationToken);
        return Ok(result);
    }

    // GET api/v1/training-histories/statistics?userId=1&startDate=...&endDate=...
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(TrainingStatisticsData[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TrainingStatisticsData[]>> GetTrainingStatistics(
        [FromQuery] int userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.GetTrainingStatistics(userId, startDate, endDate, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST api/v1/training-histories
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddTrainingHistory(
        [FromBody] SaveTrainingHistoryRequest request,
        CancellationToken cancellationToken)
    {
        await _service.AddTrainingHistory(request, cancellationToken);
        return NoContent();
    }
}