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
    public class HatsController : ControllerBase
    {
        private readonly ILogger<HatsController> _logger;

        public HatsController(ILogger<HatsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<HatResource>> Get([FromServices] api.hatApi.IHatApiClient apiClient)
        {
            _logger.LogInformation("In front-end");
            var hats = await apiClient.HatsAsync();

            return hats.Select(x => new HatResource
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
                Material = x.Material
            });
        }
    }
}