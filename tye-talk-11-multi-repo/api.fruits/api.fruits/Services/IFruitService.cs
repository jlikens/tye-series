using api.fruits.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.fruits.Services
{
    public interface IFruitService
    {
        FruitResource GetFruit(int fruitId);
        Task<FruitResource> GetFruitAsync(int fruitId);
        IEnumerable<FruitResource> GetFruits();
        Task<IEnumerable<FruitResource>> GetFruitsAsync();
    }
}
