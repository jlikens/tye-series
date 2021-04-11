using api.birds.Resources;
using api.birds.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.birds.Controllers
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
        public async Task<IEnumerable<BirdResource>> Get([FromServices] IBirdService service)
        {
            _logger.LogInformation("Getting all birds");
            return await service.GetBirdsAsync();
        }
    }
}