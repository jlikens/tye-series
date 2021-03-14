using api.books.Resources;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.books.Services
{
    public class CachingBookService : IBookService
    {
        private readonly IDistributedCache _cache;
        private readonly IBookService _dataLayer;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CachingBookService(
            IBookService dataLayer
            , IDistributedCache cache
            , IMapper mapper
            , ILogger<CachingBookService> logger)
        {
            _dataLayer = dataLayer;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        public BookResource GetBook(int bookId)
        {
            return GetBookAsync(bookId).Result;
        }

        public async Task<BookResource> GetBookAsync(int bookId)
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingBookService) + "|" + nameof(GetBookAsync);
            var key = region + bookId;
            var cached = await _cache.GetStringAsync(key);
            BookResource result;
            if (cached != null)
            {
                _logger.LogInformation("Cached book found", bookId);
                result = JsonConvert.DeserializeObject<BookResource>(cached);
            }
            else
            {
                _logger.LogInformation("Cached book not found, retrieving from data store", bookId);
                result = _dataLayer.GetBook(bookId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return _mapper.Map<BookResource>(result);
        }

        public IEnumerable<BookResource> GetBooks()
        {
            return GetBooksAsync().Result;
        }

        public async Task<IEnumerable<BookResource>> GetBooksAsync()
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingBookService) + "|" + nameof(GetBooksAsync);
            var key = region;
            var cached = await _cache.GetStringAsync(key);
            IEnumerable<BookResource> result;
            if (cached != null)
            {
                _logger.LogInformation("Cached books found");
                result = JsonConvert.DeserializeObject<List<BookResource>>(cached);
            }
            else
            {
                _logger.LogInformation("Cached books not found, retrieving from data store");
                result = await _dataLayer.GetBooksAsync();
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return result.Select(_mapper.Map<BookResource>);
        }
    }
}