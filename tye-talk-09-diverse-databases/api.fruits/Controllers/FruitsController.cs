using api.fruits.Data;
using api.fruits.Resources;
using api.fruits.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fruits.Controllers
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
        public async Task<IEnumerable<FruitResource>> Get([FromServices] IFruitService service)
        {
            _logger.LogInformation("Getting all fruits");
            return await service.GetFruitsAsync();
        }
    }
}