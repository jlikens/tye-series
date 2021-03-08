﻿using api.books.Resources;
using api.books.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.books.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public BookResource Get(int bookId, [FromServices] IBookService service)
        {
            _logger.LogInformation("Getting book", new { bookId });
            return service.GetBook(bookId);
        }
    }
}