using api.todoApi;
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
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;

        public TodoController(ILogger<TodoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItemResource>> Get([FromServices] api.todoApi.ITodoApiClient apiClient)
        {
            _logger.LogInformation("In front-end");
            var todoItems = await apiClient.TodoItemsAllAsync();

            return todoItems.Select(x => new TodoItemResource
            {
                Id = x.Id,
                IsComplete = x.IsComplete,
                Name = x.Name,
                WeatherForecast = new WeatherForecastResource
                {
                    Date = (x.WeatherForecast?.Date ?? DateTimeOffset.MinValue).LocalDateTime,
                    PostalCode = x.WeatherForecast?.PostalCode,
                    Summary = x.WeatherForecast?.Summary,
                    TemperatureC = x.WeatherForecast?.TemperatureC ?? 0,
                    TemperatureF = x.WeatherForecast?.TemperatureF ?? 0
                }
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<TodoItemResource> Post([FromBody]TodoItemResource todoItem, [FromServices] api.todoApi.ITodoApiClient apiClient)
        {
            _logger.LogInformation("In front-end");

            var toSave = new TodoItem
            {
                Name = todoItem.Name
            };
            var result = await apiClient.TodoItemsAsync(toSave);

            return new TodoItemResource
            {
                Id = result.Id,
                IsComplete = result.IsComplete,
                Name = result.Name
            };
        }
    }
}
