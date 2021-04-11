using api.hat.Data;
using api.hat.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.hat.Services
{
    public class HatService : IHatService
    {
        private readonly HatContext _hatContext;
        private readonly IMapper _mapper;

        public HatService(
            HatContext hatContext
            , IMapper mapper)
        {
            _hatContext = hatContext;
            _mapper = mapper;
        }

        public HatResource GetHat(Guid hatId)
        {
            return GetHatAsync(hatId).Result;
        }

        public async Task<HatResource> GetHatAsync(Guid hatId)
        {
            var model = await _hatContext.Hats.FirstOrDefaultAsync(s => s.Id == hatId);
            var resource = _mapper.Map<HatResource>(model);
            return resource;
        }

        public IEnumerable<HatResource> GetHats()
        {
            return GetHatsAsync().Result;
        }

        public async Task<IEnumerable<HatResource>> GetHatsAsync()
        {
            var models = await _hatContext.Hats.OrderBy(x => x.Name).ToListAsync();
            var resources = models.Select(_mapper.Map<HatResource>);
            return resources;
        }
    }
}
