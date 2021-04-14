using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace api.university.Controllers
{
    /// <summary>
    /// Quick class that demonstrates a very simple class of distributed application bug: load balanced caching.
    /// In this case, the LockBugController holds a static date that is returned for all GET requests.  This
    /// works properly in the case of a single instance of this API, but fails when more than one instance
    /// exists.  One simple approach to fixing would be to use a Redis-based IDistributed cache to synchronize
    /// the values across all instances.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LockBugController : ControllerBase
    {
        private readonly ILogger<LockBugController> _logger;
        private static DateTime _theDate = DateTime.Now;

        public LockBugController(ILogger<LockBugController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DateTime), (int)HttpStatusCode.OK)]
        public DateTime Get()
        {
            _logger.LogInformation("In LockBug Get");
            return _theDate;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DateTime), (int)HttpStatusCode.OK)]
        public ActionResult<DateTime> PostDate(DateTime theDate)
        {
            _theDate = theDate;
            return _theDate;
        }
    }
}