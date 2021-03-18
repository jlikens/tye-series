using frontend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frontend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FruitsController : ControllerBase
    {
        private readonly ILogger<FruitsController> _logger;

        public FruitsController(ILogger<FruitsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<FruitResource>> Get([FromServices] api.fruitApi.IFruitApiClient apiClient)
        {
            _logger.LogInformation("In front-end");
            var fruits = await apiClient.FruitsAsync();

            return fruits.Select(x => new FruitResource
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
                Type = x.Type,
            });
        }
    }
}