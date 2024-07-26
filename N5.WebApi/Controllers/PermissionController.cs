using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Domain.Entities;
using N5.Domain.Exceptions;
using N5.WebApi.Application.Commands.Permissions;
using N5.WebApi.Application.Handlers;
using N5.WebApi.Application.Queries;
using N5.WebApi.dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    [HttpPost("RequestPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePermission([FromBody] CreateRequestPermissionDto permission)
    {
        var response = await _mediator.Send(new CreatePermissionCommand
        {
            EmployeeName = permission.EmployeeName,
            EmployeeSurname = permission.LastName,
            PermissionDate = permission.Date,
            PermissionType = permission.TypePermissionId
        });
        return await HomologateResponse(response);
    }

    [HttpPost("ModifyPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ModifyPermission([FromBody] UpdateRequestPermissionDto permission)
    {
        var response = await _mediator.Send(new UpdatePermissionCommand
        {
            IdPermission = permission.IdPermission,
            PermissionDate = permission.Date,
            PermissionType = permission.TypePermissionId
        });
        return await HomologateResponse(response);

    }


    [HttpPost("GetPermissions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPermission()
    {
        GetPermissionsQuery query = new GetPermissionsQuery();
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    private async Task<ActionResult> HomologateResponse(ResponseMessageDto<Permission> response)
    {
        switch (response.StatusCode)
        {
            case StatusCodes.Status200OK:
            default:
                return Ok(response);
            case StatusCodes.Status500InternalServerError:
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            case StatusCodes.Status400BadRequest:
                return BadRequest(response);
        }
    }
}
