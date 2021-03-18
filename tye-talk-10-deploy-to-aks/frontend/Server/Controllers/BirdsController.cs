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
    public class BirdsController : ControllerBase
    {
        private readonly ILogger<BirdsController> _logger;

        public BirdsController(ILogger<BirdsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<BirdResource>> Get([FromServices] api.birdApi.IBirdApiClient apiClient)
        {
            _logger.LogInformation("In front-end");
            var birds = await apiClient.BirdsAsync();

            return birds.Select(x => new BirdResource
            {
                Id = x.Id,
                Name = x.Name,
                WingSpan = x.WingSpan,
            });
        }
    }
}