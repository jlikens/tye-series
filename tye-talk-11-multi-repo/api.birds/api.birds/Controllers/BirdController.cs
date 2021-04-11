using api.birds.Resources;
using api.birds.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace api.birds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BirdController : ControllerBase
    {
        private readonly ILogger<BirdController> _logger;

        public BirdController(ILogger<BirdController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<BirdResource> Get(Guid birdId, [FromServices] IBirdService service)
        {
            _logger.LogInformation("Getting bird", new { birdId });
            return await service.GetBirdAsync(birdId);
        }
    }
}