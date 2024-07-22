using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Domain.Entities;
using N5.Infraestructure.Interfaces;
using N5.WebApi.Application.Commands.Permissions;

namespace N5.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult> Get()
        {
            return StatusCode(StatusCodes.Status201Created, await _mediator.Send(new CreatePermissionCommand { 
             EmployeeName = "sergio",
              EmployeeSurname = "fernandez",
               PermissionDate =  DateTime.Now,
                PermissionType =  1
            }));
           
        }
    }
}
