using api.birds.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.birds.Services
{
    public interface IBirdService
    {
        BirdResource GetBird(int birdId);
        Task<BirdResource> GetBirdAsync(int birdId);
        IEnumerable<BirdResource> GetBirds();
        Task<IEnumerable<BirdResource>> GetBirdsAsync();
    }
}
