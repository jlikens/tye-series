using api.hat.Resources;
using api.hat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace api.hat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HatController : ControllerBase
    {
        private readonly ILogger<HatController> _logger;

        public HatController(ILogger<HatController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<HatResource> Get(Guid hatId, [FromServices] IHatService service)
        {
            _logger.LogInformation("Getting hat", new { hatId });
            return await service.GetHatAsync(hatId);
        }
    }
}