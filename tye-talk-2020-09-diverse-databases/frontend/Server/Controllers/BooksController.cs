using frontend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frontend.Server.Controllers
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
        public async Task<IEnumerable<BookResource>> Get([FromServices] api.bookApi.IBookApiClient apiClient)
        {
            _logger.LogInformation("In front-end");
            var books = await apiClient.BooksAsync();

            return books.Select(x => new BookResource
            {
                Id = x.Id,
                Author = new AuthorResource
                {
                    FirstMidName = x.Author.FirstMidName,
                    FullName = x.Author.FullName,
                    Id = x.Author.Id,
                },
                Genre = x.Genre,
                PageCount = x.PageCount,
                PublishDate = x.PublishDate.LocalDateTime,
                Title = x.Title
            });
        }
    }
}