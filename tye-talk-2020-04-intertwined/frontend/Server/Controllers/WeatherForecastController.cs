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
        public async Task<IEnumerable<WeatherForecastResource>> Get()
        {
            _logger.LogInformation("In front-end: getting weather");
            var httpClient = new HttpClient();
            var client = new api.weatherApi.WeatherApiClient("https://localhost:5003", httpClient);
            var forecast = await client.WeatherForecastAsync();

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
