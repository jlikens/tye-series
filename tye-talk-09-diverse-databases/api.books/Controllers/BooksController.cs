using api.books.Data;
using api.books.Resources;
using api.books.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.books.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<BookResource>> Get([FromServices] IBookService service)
        {
            _logger.LogInformation("Getting all books");
            return await service.GetBooksAsync();
        }
    }
}