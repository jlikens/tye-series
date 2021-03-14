using api.university.Resources;
using api.university.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public StudentResource Get(int studentId, [FromServices] IStudentService service)
        {
            _logger.LogInformation("Getting student", new { studentId });
            return service.GetStudent(studentId);
        }
    }
}