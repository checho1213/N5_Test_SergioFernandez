using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.WebApi.Application.Commands.Permissions;
using N5.WebApi.dto;

namespace N5.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PermissionController : ControllerBase
{
    private readonly ILogger<PermissionController> _logger;
    private readonly IMediator _mediator;

    public PermissionController(ILogger<PermissionController> logger, IMediator mediator)
    {
        _logger = logger;
        this._mediator = mediator;
    }

    [HttpPost(Name = "CreatePermission")]
    public async Task<ActionResult> CreatePermission([FromBody] PermissionDto permission)
    {
        return StatusCode(StatusCodes.Status201Created, await _mediator.Send(new CreatePermissionCommand
        {
            EmployeeName = permission.EmployeeName,
            EmployeeSurname = permission.LastName,
            PermissionDate = permission.Date,
            PermissionType = permission.TypePermissionId
        }));

    }
}
