using api.fruits.Data;
using api.fruits.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fruits.Services
{
    public class FruitService : IFruitService
    {
        private readonly FruitContext _fruitContext;
        private readonly IMapper _mapper;

        public FruitService(
            FruitContext fruitContext
            , IMapper mapper)
        {
            _fruitContext = fruitContext;
            _mapper = mapper;
        }

        public FruitResource GetFruit(int fruitId)
        {
            return GetFruitAsync(fruitId).Result;
        }

        public async Task<FruitResource> GetFruitAsync(int fruitId)
        {
            var model = await _fruitContext.Fruit.FirstOrDefaultAsync(s => s.Id == fruitId);
            var resource = _mapper.Map<FruitResource>(model);
            return resource;
        }

        public IEnumerable<FruitResource> GetFruits()
        {
            return GetFruitsAsync().Result;
        }

        public async Task<IEnumerable<FruitResource>> GetFruitsAsync()
        {
            var models = await _fruitContext.Fruit.OrderBy(x => x.Name).ToListAsync();
            var resources = models.Select(_mapper.Map<FruitResource>);
            return resources;
        }
    }
}
