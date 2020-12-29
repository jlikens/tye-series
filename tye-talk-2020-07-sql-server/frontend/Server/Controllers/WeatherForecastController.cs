using frontend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace frontend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastResource>> Get([FromServices] api.weatherApi.IWeatherApiClient apiClient)
        {
            _logger.LogInformation("In front-end: getting weather");
            var forecast = await apiClient.WeatherForecastAsync();

            return forecast.Select(x => new WeatherForecastResource
            {
                Date = x.Date.LocalDateTime,
                PostalCode = x.PostalCode,
                TemperatureC = x.TemperatureC,
                Summary = x.Summary
            })
            .ToArray();
        }
    }
}
