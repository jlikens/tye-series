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
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentResource>> Get([FromServices] api.universityApi.IUniversityApiClient apiClient)
        {
            _logger.LogInformation("In front-end");
            var students = await apiClient.StudentsAsync();

            return students.Select(x => new StudentResource
            {
                Id = x.Id,
                FullName = x.FullName
            });
        }
    }
}