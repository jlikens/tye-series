using api.birds.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.birds.Services
{
    public class BirdService : IBirdService
    {
        private readonly IConfig _config;
        private readonly IMapper _mapper;

        public BirdService(IConfig config
            , IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        public BirdResource GetBird(Guid birdId)
        {
            return GetBirdAsync(birdId).Result;
        }

        public async Task<BirdResource> GetBirdAsync(Guid birdId)
        {
            var client = new MongoClient(_config.MongoDbConnectionString);
            var database = client.GetDatabase("birdsApi");
            var birdCursor = await database.GetCollection<Models.Bird>("birds").FindAsync<Models.Bird>(x => x.Id == birdId);
            return _mapper.Map<BirdResource>(await birdCursor.FirstOrDefaultAsync());
        }

        public IEnumerable<BirdResource> GetBirds()
        {
            return GetBirdsAsync().Result;
        }

        public async Task<IEnumerable<BirdResource>> GetBirdsAsync()
        {
            var client = new MongoClient(_config.MongoDbConnectionString);
            var database = client.GetDatabase("birdsApi");
            var birdCursor = await database.GetCollection<Models.Bird>("birds").FindAsync<Models.Bird>(_ => true); 
            var birds = await birdCursor.ToListAsync();
            return birds.Select(_mapper.Map<BirdResource>);
        }
    }
}
