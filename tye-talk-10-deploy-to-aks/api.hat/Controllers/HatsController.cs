using api.hat.Data;
using api.hat.Resources;
using api.hat.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.hat.Controllers
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
        public async Task<IEnumerable<HatResource>> Get([FromServices] IHatService service)
        {
            _logger.LogInformation("Getting all hats");
            return await service.GetHatsAsync();
        }
    }
}