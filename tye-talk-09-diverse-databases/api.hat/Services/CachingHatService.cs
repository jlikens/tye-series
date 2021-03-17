using api.hat.Resources;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.hat.Services
{
    public class CachingHatService : IHatService
    {
        private readonly IDistributedCache _cache;
        private readonly IHatService _dataLayer;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CachingHatService(
            IHatService dataLayer
            , IDistributedCache cache
            , IMapper mapper
            , ILogger<CachingHatService> logger)
        {
            _dataLayer = dataLayer;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        public HatResource GetHat(Guid hatId)
        {
            return GetHatAsync(hatId).Result;
        }

        public async Task<HatResource> GetHatAsync(Guid hatId)
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingHatService) + "|" + nameof(GetHatAsync);
            var key = region + hatId;
            var cached = await _cache.GetStringAsync(key);
            HatResource result;
            if (cached != null)
            {
                _logger.LogInformation("Cached hat found", hatId);
                result = JsonConvert.DeserializeObject<HatResource>(cached);
            }
            else
            {
                _logger.LogInformation("Cached hat not found, retrieving from data store", hatId);
                result = _dataLayer.GetHat(hatId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return _mapper.Map<HatResource>(result);
        }

        public IEnumerable<HatResource> GetHats()
        {
            return GetHatsAsync().Result;
        }

        public async Task<IEnumerable<HatResource>> GetHatsAsync()
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingHatService) + "|" + nameof(GetHatsAsync);
            var key = region;
            var cached = await _cache.GetStringAsync(key);
            IEnumerable<HatResource> result;
            if (cached != null)
            {
                _logger.LogInformation("Cached hats found");
                result = JsonConvert.DeserializeObject<List<HatResource>>(cached);
            }
            else
            {
                _logger.LogInformation("Cached hats not found, retrieving from data store");
                result = await _dataLayer.GetHatsAsync();
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return result.Select(_mapper.Map<HatResource>);
        }
    }
}