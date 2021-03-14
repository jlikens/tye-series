using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using api.clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.person.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly ITodoApiClient _todoApiClient;

        public PersonController(PersonContext context, api.clients.ITodoApiClient todoApiClient)
        {
            _context = context;
            _todoApiClient = todoApiClient;
        }

        // GET: api/Person
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Person>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var personsDb = await _context.Persons.ToListAsync();
            return personsDb.Select(async x => await ToResource(x)).Select(x => x.Result).ToList();
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            var personDb = await _context.Persons.FindAsync(id);

            if (personDb == null)
            {
                return NotFound();
            }
            return await ToResource(personDb);
        }

        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Person>> PutPerson(Guid id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await GetPerson(id);
        }

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            var personDb = ToDb(person);
            _context.Persons.Add(personDb);
            await _context.SaveChangesAsync();

            return await GetPerson(personDb.Id);
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(Guid id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

        private async Task<IEnumerable<TodoItem>> GetRandomTodoItems()
        {
            var rnd = new Random();
            var todoItems = await _todoApiClient.TodoItemsAllAsync();
            return todoItems.Take(rnd.Next(0, todoItems.Count() + 1));
        }


        private async Task<Person> ToResource(PersonDb dbItem)
        {
            return new Person
            {
                EmailAddress = dbItem.EmailAddress,
                FirstName = dbItem.FirstName,
                Id = dbItem.Id,
                LastName = dbItem.LastName,
                Password = dbItem.Password,
                TodoItems = await GetRandomTodoItems()
            };
        }

        private PersonDb ToDb(Person person)
        {
            return new PersonDb
            {
                EmailAddress = person.EmailAddress,
                FirstName = person.FirstName,
                Id = person.Id,
                LastName = person.LastName,
                Password = person.Password
            };
        }
    }
}
