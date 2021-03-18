using api.hat.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.hat.Services
{
    public interface IHatService
    {
        HatResource GetHat(Guid hatId);
        Task<HatResource> GetHatAsync(Guid hatId);
        IEnumerable<HatResource> GetHats();
        Task<IEnumerable<HatResource>> GetHatsAsync();
    }
}
