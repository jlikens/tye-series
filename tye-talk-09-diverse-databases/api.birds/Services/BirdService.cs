using api.birds.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.birds.Services
{
    //public class BirdService : IBirdService
    //{
    //    private readonly BirdContext _birdContext;
    //    private readonly IMapper _mapper;

    //    public BirdService(
    //        BirdContext birdContext
    //        , IMapper mapper)
    //    {
    //        _birdContext = birdContext;
    //        _mapper = mapper;
    //    }

    //    public BirdResource GetBird(int birdId)
    //    {
    //        return GetBirdAsync(birdId).Result;
    //    }

    //    public async Task<BirdResource> GetBirdAsync(int birdId)
    //    {
    //        var model = await _birdContext.Birds.FirstOrDefaultAsync(s => s.Id == birdId);
    //        var resource = _mapper.Map<BirdResource>(model);
    //        return resource;
    //    }

    //    public IEnumerable<BirdResource> GetBirds()
    //    {
    //        return GetBirdsAsync().Result;
    //    }

    //    public async Task<IEnumerable<BirdResource>> GetBirdsAsync()
    //    {
    //        var models = await _birdContext.Birds.OrderBy(x => x.Name).ToListAsync();
    //        var resources = models.Select(_mapper.Map<BirdResource>);
    //        return resources;
    //    }
    //}
}
