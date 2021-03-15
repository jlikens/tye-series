using api.fruits.Resources;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fruits.Services
{
    public class CachingFruitService : IFruitService
    {
        private readonly IDistributedCache _cache;
        private readonly IFruitService _dataLayer;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CachingFruitService(
            IFruitService dataLayer
            , IDistributedCache cache
            , IMapper mapper
            , ILogger<CachingFruitService> logger)
        {
            _dataLayer = dataLayer;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        public FruitResource GetFruit(int fruitId)
        {
            return GetFruitAsync(fruitId).Result;
        }

        public async Task<FruitResource> GetFruitAsync(int fruitId)
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingFruitService) + "|" + nameof(GetFruitAsync);
            var key = region + fruitId;
            var cached = await _cache.GetStringAsync(key);
            FruitResource result;
            if (cached != null)
            {
                _logger.LogInformation("Cached fruit found", fruitId);
                result = JsonConvert.DeserializeObject<FruitResource>(cached);
            }
            else
            {
                _logger.LogInformation("Cached fruit not found, retrieving from data store", fruitId);
                result = _dataLayer.GetFruit(fruitId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return _mapper.Map<FruitResource>(result);
        }

        public IEnumerable<FruitResource> GetFruits()
        {
            return GetFruitsAsync().Result;
        }

        public async Task<IEnumerable<FruitResource>> GetFruitsAsync()
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingFruitService) + "|" + nameof(GetFruitsAsync);
            var key = region;
            var cached = await _cache.GetStringAsync(key);
            IEnumerable<FruitResource> result;
            if (cached != null)
            {
                _logger.LogInformation("Cached fruits found");
                result = JsonConvert.DeserializeObject<List<FruitResource>>(cached);
            }
            else
            {
                _logger.LogInformation("Cached fruits not found, retrieving from data store");
                result = await _dataLayer.GetFruitsAsync();
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return result.Select(_mapper.Map<FruitResource>);
        }
    }
}