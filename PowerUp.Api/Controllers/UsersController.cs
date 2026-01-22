using Microsoft.AspNetCore.Mvc;
using PowerUp.Application.Services.Users;

namespace PowerUp.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly UsersService _service;

    public UsersController(UsersService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var user = await _service.GetById(id, cancellationToken);

        return Ok(user);
    }
}