using api.university.Data;
using api.university.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.university.Controllers
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
        public IEnumerable<StudentResource> Get([FromServices] SchoolContext dbContext, [FromServices] IMapper mapper, [FromServices]IConfiguration config)
        {
            _logger.LogInformation("Getting all students");
            return new StudentResource[]
            {
                new StudentResource
                {
                    FirstMidName = config.GetConnectionString("sqlserver-usidore") ?? "name=University",
                    LastName = config.GetConnectionString("sqlserver-usidore") ?? "name=University",
                    FullName = config.GetConnectionString("sqlserver-usidore") ?? "name=University",
                }
            };
            var models = dbContext.Students.OrderBy(s => s.LastName + s.FirstMidName);
            var resources = models.Select(m => mapper.Map<StudentResource>(m)).ToList();
            return resources;
        }
    }
}