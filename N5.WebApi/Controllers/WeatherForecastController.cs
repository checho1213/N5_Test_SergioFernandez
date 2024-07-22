using Microsoft.AspNetCore.Mvc;
using N5.Domain.Entities;
using N5.Infraestructure.Interfaces;

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
        private readonly IUnitofWork unitofWork;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            this.unitofWork = unitofWork;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Permisos> Get()
        {
           return unitofWork.PermisosRepository.GetAll();
        }
    }
}
