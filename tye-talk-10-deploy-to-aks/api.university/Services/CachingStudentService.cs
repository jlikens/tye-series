using api.university.Resources;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace api.university.Services
{
    public class CachingStudentService : IStudentService
    {
        private readonly IDistributedCache _cache;
        private readonly IStudentService _dataLayer;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CachingStudentService(
            IStudentService dataLayer
            , IDistributedCache cache
            , IMapper mapper
            , ILogger<CachingStudentService> logger)
        {
            _dataLayer = dataLayer;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        public StudentResource GetStudent(int studentId)
        {
            return GetStudentAsync(studentId).Result;
        }

        public async Task<StudentResource> GetStudentAsync(int studentId)
        {
            // Note: In the real world, we'd use a caching interface that supports GetOrAddAsync<T> for atomicity
            const string region = nameof(CachingStudentService) + "|" + nameof(GetStudentAsync);
            var key = region + studentId;
            var cached = await _cache.GetStringAsync(key);
            StudentResource result;
            if(cached != null)
            {
                _logger.LogInformation("Cached student found", studentId);
                result = JsonConvert.DeserializeObject<StudentResource>(cached);
            }
            else
            {
                _logger.LogInformation("Cached student not found, retrieving from data store", studentId);
                result = _dataLayer.GetStudent(studentId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result));
            }

            return _mapper.Map<StudentResource>(result);
        }
    }
}