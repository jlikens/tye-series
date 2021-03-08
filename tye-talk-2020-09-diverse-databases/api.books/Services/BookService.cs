using api.books.Data;
using api.books.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.books.Services
{
    public class BookService : IBookService
    {
        private readonly BookContext _bookContext;
        private readonly IMapper _mapper;

        public BookService(
            BookContext bookContext
            , IMapper mapper)
        {
            _bookContext = bookContext;
            _mapper = mapper;
        }

        public BookResource GetBook(int bookId)
        {
            return GetBookAsync(bookId).Result;
        }

        public async Task<BookResource> GetBookAsync(int bookId)
        {
            var model = await _bookContext.Books.Include(x => x.Author).FirstOrDefaultAsync(s => s.ID == bookId);
            var resource = _mapper.Map<BookResource>(model);
            return resource;
        }

        public IEnumerable<BookResource> GetBooks()
        {
            return GetBooksAsync().Result;
        }

        public async Task<IEnumerable<BookResource>> GetBooksAsync()
        {
            var models = await _bookContext.Books.Include(x => x.Author).OrderBy(x => x.Author).ThenBy(x => x.Title).ToListAsync();
            var resources = models.Select(_mapper.Map<BookResource>);
            return resources;
        }
    }
}
