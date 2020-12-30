using api.university.Data;
using api.university.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace api.university.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public StudentResource Get(int studentId, [FromServices] SchoolContext dbContext, [FromServices] IMapper mapper)
        {
            _logger.LogInformation("Getting student", new { studentId });
            var model = dbContext.Students.FirstOrDefault(s => s.ID == studentId);
            var resource = mapper.Map<StudentResource>(model);
            return resource;
        }
    }
}