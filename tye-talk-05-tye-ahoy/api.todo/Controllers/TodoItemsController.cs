using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using api.client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.todo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IWeatherApiClient _weatherApiClient;

        public TodoItemsController(TodoContext context, api.client.IWeatherApiClient weatherApiClient)
        {
            _context = context;
            _weatherApiClient = weatherApiClient;
        }

        // GET: api/TodoItems
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var todoItemsDb = await _context.TodoItems.ToListAsync();
            return todoItemsDb.Select(async x => await ToResource(x)).Select(x => x.Result).ToList();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItemDb = await _context.TodoItems.FindAsync(id);

            if (todoItemDb == null)
            {
                return NotFound();
            }
            return await ToResource(todoItemDb);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TodoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await GetTodoItem(id);
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            var todoItemDb = ToDb(todoItem);
            _context.TodoItems.Add(todoItemDb);
            await _context.SaveChangesAsync();

            return await GetTodoItem(todoItemDb.Id);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private async Task<WeatherForecast> GetRandomWeatherForecast()
        {
            var rnd = new Random();
            var weatherForecasts = await _weatherApiClient.WeatherForecastAsync();
            return weatherForecasts.ToArray()[rnd.Next(0, weatherForecasts.Count())];
        }

        private async Task<TodoItem> ToResource(TodoItemDb dbItem)
        {
            return new TodoItem
            {
                Id = dbItem.Id,
                IsComplete = dbItem.IsComplete,
                Name = dbItem.Name,
                WeatherForecast = await GetRandomWeatherForecast()
            };
        }

        private TodoItemDb ToDb(TodoItem todoItem)
        {
            return new TodoItemDb
            {
                Id = todoItem.Id,
                IsComplete = todoItem.IsComplete,
                Name = todoItem.Name
            };
        }
    }
}
