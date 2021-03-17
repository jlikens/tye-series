using api.fruits.Resources;
using api.fruits.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.fruits.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FruitController : ControllerBase
    {
        private readonly ILogger<FruitController> _logger;

        public FruitController(ILogger<FruitController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public FruitResource Get(int fruitId, [FromServices] IFruitService service)
        {
            _logger.LogInformation("Getting fruit", new { fruitId });
            return service.GetFruit(fruitId);
        }
    }
}