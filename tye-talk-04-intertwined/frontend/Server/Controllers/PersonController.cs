﻿using api.personApi;
using frontend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace frontend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonResource>> Get()
        {
            _logger.LogInformation("In front-end");
            var httpClient = new HttpClient();
            var client = new api.personApi.PersonApiClient("https://localhost:5007", httpClient);
            var people = await client.PersonAllAsync();

            return people.Select(x => new PersonResource
            {
                EmailAddress = x.EmailAddress,
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName,
                Password = x.Password,
                TodoItems = x?.TodoItems?.Select(t => new TodoItemResource
                {
                    Id = t.Id,
                    IsComplete = t.IsComplete,
                    Name = t.Name,
                    WeatherForecast = new WeatherForecastResource
                    {
                        Date = t.WeatherForecast.Date.LocalDateTime,
                        Summary = t.WeatherForecast.Summary,
                        TemperatureC = t.WeatherForecast.TemperatureC,
                        TemperatureF = t.WeatherForecast.TemperatureF,
                        PostalCode = t.WeatherForecast.PostalCode
                    }
                })

            })
            .ToArray();
        }

        [HttpPost]
        public async Task<PersonResource> Post([FromBody] PersonResource person)
        {
            _logger.LogInformation("In front-end");
            var httpClient = new HttpClient();
            var client = new api.personApi.PersonApiClient("https://localhost:5007", httpClient);

            var toSave = new Person
            {
                EmailAddress = person.EmailAddress,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Password= person.Password
            };
            var result = await client.PersonAsync(toSave);

            return new PersonResource
            {
                EmailAddress = result.EmailAddress,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Password = result.Password
            };
        }
    }
}
