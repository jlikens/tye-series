using api.books.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.books.Services
{
    public interface IBookService
    {
        BookResource GetBook(int bookId);
        Task<BookResource> GetBookAsync(int bookId);
        IEnumerable<BookResource> GetBooks();
        Task<IEnumerable<BookResource>> GetBooksAsync();
    }
}
