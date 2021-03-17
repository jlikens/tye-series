using api.birds.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.birds.Services
{
    public interface IBirdService
    {
        BirdResource GetBird(Guid birdId);
        Task<BirdResource> GetBirdAsync(Guid birdId);
        IEnumerable<BirdResource> GetBirds();
        Task<IEnumerable<BirdResource>> GetBirdsAsync();
    }
}
