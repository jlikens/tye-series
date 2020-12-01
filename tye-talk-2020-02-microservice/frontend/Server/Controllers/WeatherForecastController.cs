﻿using frontend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using api.weather;

namespace frontend.Server.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastResource>> Get()
        {
            var httpClient = new HttpClient();
            var client = new WeatherApiClient("https://localhost:44361", httpClient);
            var forecast = await client.WeatherForecastAsync();

            return forecast.Select(x => new WeatherForecastResource
            {
                Date = x.Date.LocalDateTime,
                TemperatureC = x.TemperatureC,
                Summary = x.Summary
            })
            .ToArray();
        }
    }
}
